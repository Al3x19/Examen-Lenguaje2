using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaContable.API.Database;
using SistemaContable.API.Database.Entities;
using SistemaContable.API.Dtos.Accounts;
using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Dtos.JournalEntries;
using SistemaContable.API.Dtos.Movements;
using SistemaContable.API.Services.Interfaces;

namespace SistemaContable.API.Services
{
    public class JournalService : IJournalService
    {
        private readonly SistemaContableContext _context;
        private readonly IMapper _mapper;
        private readonly LogsContext _logsContext;
        private readonly ILogger<JournalService> _logger;
        private readonly IAuthService _authService;
        private readonly int PAGE_SIZE;

        public JournalService(
            SistemaContableContext context,
            IMapper mapper,
            LogsContext logsContext,
            ILogger<JournalService> logger,
            IAuthService authService,
            IConfiguration configuration
            )
        {
            this._context = context;
            this._mapper = mapper;
            this._logsContext = logsContext;
            this._logger = logger;
            this._authService = authService;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
        }

        async Task<ResponseDto<PaginationDto<List<JournalEntryDto>>>> IJournalService.GetJournalListAsync(string searchTerm, int page)
        {
            int startIndex = (page - 1) * PAGE_SIZE;

            var journalEntityQuery = _context.JournalEntries
                .Where(x => x.CreatedByUser.UserName.Contains(searchTerm.ToLower()) && x.IsHidden != true);

            int totalCategories = await journalEntityQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCategories / PAGE_SIZE);

            var entriesEntity = await journalEntityQuery
                .OrderBy(u => u.Number)
                .Skip(startIndex)
                .Take(PAGE_SIZE)
                .ToListAsync();


            var entriesDtos = _mapper.Map<List<JournalEntryDto>>(entriesEntity);
            var i = 0;
            while (i < entriesEntity.Count)
            {
                if (entriesDtos[i].Movements is null)
                {
                    entriesDtos[i].Movements = new List<MovementDto>();
                }
                var entitymoves = _context.Movements.Where(x=> x.JournalId == entriesEntity[i].Number).ToList();
                var dtolist = _mapper.Map<List<MovementDto>>(entitymoves);
                var u = 0;
                while(u<entitymoves.Count)
                {
                    dtolist[u].Account = _mapper.Map<AccountDto>(_context.Accounts.FirstOrDefault(x => x.Code == entitymoves[u].AccountCode));
                    u++;
                }

                entriesDtos[i].User = _context.Users.FirstOrDefault(x => x.Id== entriesEntity[i].CreatedBy).UserName;
                
                entriesDtos[i].Movements.AddRange(dtolist);
                i++;
            }

            var log1 = new LogsEntity
            {
                table = "journal_entries",
                action = "get",
                oldValue = null,
                newValue = searchTerm
            };

            _logsContext.Logs.Add(log1);
            await _logsContext.SaveChangesAsync();

            return new ResponseDto<PaginationDto<List<JournalEntryDto>>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Listado de partidas obtenido...",
                Data = new PaginationDto<List<JournalEntryDto>>
                {
                    CurrentPage = page,
                    PageSize = PAGE_SIZE,
                    TotalItems = totalCategories,
                    TotalPages = totalPages,
                    Items = entriesDtos,
                    HasPreviousPage = page > 1,
                    HasNextPage = page < totalPages,
                }
            };

        }

        async Task<ResponseDto<JournalEntryDto>> IJournalService.CreateAsync(JournalEntryCreateDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var journalEntryEntity = _mapper.Map<JournalEntryEntity>(dto);
                    var debe = dto.Movements.Select(x => x.Debit).ToList();
                    var haber = dto.Movements.Select(x => x.Credit).ToList();

                    if (haber.Sum() != debe.Sum())
                    {
                        return new ResponseDto<JournalEntryDto>
                        {
                            StatusCode = 403,
                            Status = false,
                            Message = "El debe y el haber deben cuadrar..."
                        };
                    }

                    
                    //foreach para valores de cuenta...
                    foreach (var move in dto.Movements)
                    {
                        var accountEntity = await _context.Accounts.Where(u => u.IsBlocked==false).FirstOrDefaultAsync(x => x.Code == move.AccountCode);
                        if (accountEntity is null)
                        {
                            return new ResponseDto<JournalEntryDto>
                            {
                                StatusCode = 404,
                                Status = false,
                                Message = "Cuenta no encontrada"
                            };
                        }

                        if (!accountEntity.AllowMovement)
                        {
                            return new ResponseDto<JournalEntryDto>
                            {
                                StatusCode = 403,
                                Status = false,
                                Message = "Esta cuenta no acepta movimientos"
                            };
                        }
                        var balanceEntity = await _context.Balances.FirstOrDefaultAsync(x => x.AccountCode == accountEntity.Code);
                        if (balanceEntity is null || balanceEntity.Year != DateTime.Now.Year || balanceEntity.Month != DateTime.Now.Month)
                        {
                            balanceEntity = new BalanceEntity
                            {
                                Amount = 0,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                AccountCode = accountEntity.Code,
                            };                       
                            _context.Balances.Add(balanceEntity);
                            await _context.SaveChangesAsync();
                            var log1 = new LogsEntity
                            {
                                table = "balances",
                                action = "post",
                                oldValue = null,
                                newValue = JsonConvert.SerializeObject(balanceEntity)
                            };
                            _logsContext.Logs.Add(log1);

                            await _logsContext.SaveChangesAsync();
                        }

                        var OGbalance = await _context.Balances.FirstOrDefaultAsync(x => x.AccountCode == accountEntity.Code);
                        do
                        {
                            var balance = await _context.Balances.FirstOrDefaultAsync(x => x.AccountCode == accountEntity.Code);
                            if (balance != null)
                            {
                                balance.Amount += move.Debit - move.Credit;
                            }
                            _context.Balances.Update(balance);
                            accountEntity = accountEntity.ParentAccount;


                        } while (accountEntity != null);
                        await _context.SaveChangesAsync();

                      
                        var log2 = new LogsEntity
                        {
                            table = "balances",
                            action = "put",
                            oldValue = JsonConvert.SerializeObject(balanceEntity),
                            newValue = JsonConvert.SerializeObject(OGbalance)
                        };
                        _logsContext.Logs.Add(log2);

                        await _logsContext.SaveChangesAsync();

                    }
                    await _context.SaveChangesAsync();

                    await _context.JournalEntries.AddAsync(journalEntryEntity);
                    await _context.SaveChangesAsync();

                    var journalDto = _mapper.Map<JournalEntryDto>(journalEntryEntity);
                    var log3 = new LogsEntity
                    {
                        table = "journal_entries",
                        action = "post",
                        oldValue = null,
                        newValue = JsonConvert.SerializeObject(journalDto)
                    };

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();


                     _logsContext.Logs.Add(log3);

                    await _logsContext.SaveChangesAsync();


                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 201,
                        Status = false,
                        Message = "Partida creada correctamente...",
                        Data = journalDto

                    };

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error al crear partida...");
                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = "Error al crear la partida..."
                    };
                }
            
            }
        }


        public async Task<ResponseDto<JournalEntryDto>> UpdateAsync(JournalEntryUpdateDto dto, int number)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var journalentryEntity = await _context.JournalEntries.FirstOrDefaultAsync(x => x.Number == number);


                    if (journalentryEntity == null || journalentryEntity.IsHidden == true)
                    {
                        return new ResponseDto<JournalEntryDto>
                        {
                            StatusCode = 404,
                            Status = false,
                            Message = "Cuenta no encontrada..."
                        };
                    }
                    var oldvalue = journalentryEntity;
                    _mapper.Map<JournalEntryUpdateDto, JournalEntryEntity>(dto, journalentryEntity);


                    _context.JournalEntries.Update(journalentryEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var log1 = new LogsEntity
                    {
                        table = "journal_entries",
                        action = "put",
                        oldValue = JsonConvert.SerializeObject(oldvalue),
                        newValue = JsonConvert.SerializeObject(journalentryEntity)
                    };

                    _logsContext.Logs.Add(log1);
                    await _logsContext.SaveChangesAsync();

                    var accountDto = _mapper.Map<AccountDto>(journalentryEntity);
                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 200,
                        Status = true,
                        Message = "Editado correctamente..."
                    };


                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error al modificar cuenta...");
                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = "Error al modificar la partida..."
                    };
                }

            }



        }

        public async Task<ResponseDto<JournalEntryDto>> DeleteAsync(int number)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var entryEntity = _context.JournalEntries.FirstOrDefault(x => x.Number == number);

                    if (entryEntity == null || entryEntity.IsHidden == true)
                    {
                        return new ResponseDto<JournalEntryDto>
                        {
                            StatusCode = 404,
                            Status = false,
                            Message = "Partida no encontrada..."
                        };
                    }
                    var oldvalue = entryEntity;
                    entryEntity.IsHidden = true;

                    _context.JournalEntries.Update(entryEntity);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();


                    var log1 = new LogsEntity
                    {
                        table = "journal_entries",
                        action = "delete",
                        oldValue = JsonConvert.SerializeObject(oldvalue),
                        newValue = JsonConvert.SerializeObject(entryEntity)
                    };

                    _logsContext.Logs.Add(log1);
                    await _logsContext.SaveChangesAsync();

                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 200,
                        Status = true,
                        Message = "Borrado exitosamente..."
                    };

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "No pudo completarse la eliminacion...");
                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = "No pudo completarse la eliminacion..."
                    };
                }

            }
        }
    }
}
