using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SistemaContable.API.Database;
using SistemaContable.API.Database.Entities;
using SistemaContable.API.Dtos.Accounts;
using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Dtos.Logs;
using SistemaContable.API.Dtos.Movements;
using SistemaContable.API.Services.Interfaces;

namespace SistemaContable.API.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly SistemaContableContext _context;
        private readonly LogsContext _logcontext;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsService> _logger;
        private readonly IAuthService _authService;
        private readonly int PAGE_SIZE;

        public AccountsService(
            SistemaContableContext context,
            LogsContext logcontext,
            IMapper mapper,
            ILogger<AccountsService> logger,
            IAuthService authService,
            IConfiguration configuration
            )
        {
            this._context = context;
            this._logcontext = logcontext;
            this._mapper = mapper;
            this._logger = logger;
            this._authService = authService;

        }


        public async Task<ResponseDto<AccountDto>> CreateAsync(AccountCreateDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //Creacion de entidad en base a dto, y ajustar si acepta movimiento como true....
                    var accountEntity = _mapper.Map<AccountEntity>(dto);
                    accountEntity.AllowMovement = true;
                    accountEntity.IsBlocked = false;

                    var namecheck = await _context.Accounts.FirstOrDefaultAsync(x=> x.Name.Trim() == dto.Name.Trim());
                    if(namecheck != null)
                    {
                        return new ResponseDto<AccountDto>
                        {
                            StatusCode = 403,
                            Status = false,
                            Message = "El nombre ya es utilizado..."
                        };
                    }

                    //--------------------------------------------------------------------------------------------------------------------------------
                    //Validacion en caso de que la cuenta tenga padre
                    if (accountEntity.ParentAccountCode != null)
                    {
                        //Creacion y validacion de entidad de padre... 
                        var parentEntity = await _context.Accounts.FirstOrDefaultAsync(x => x.Code == accountEntity.ParentAccountCode);


                        if (parentEntity is null || parentEntity.IsBlocked == true)
                        {
                            return new ResponseDto<AccountDto>
                            {
                                StatusCode = 404,
                                Status = false,
                                Message = "No se encontro la cuenta padre"
                            };
                        }

                        //Automaticacion de codigo de cuenta con padre...
                        var accountlist = _context.Accounts.Where(x => x.ParentAccountCode == accountEntity.ParentAccountCode);
                        accountEntity.Code = accountEntity.ParentAccountCode + "." + (accountlist.Count() + 1).ToString();

                        //-------------------------------------------------------------------------------------------------------------------------------------------------------
                        //Validacion en caso sea el primer hijo...
                        if (parentEntity.AllowMovement)
                        {
                            //obtener el balance actual del padre..
                            var padrebalance = await _context.Balances.FirstOrDefaultAsync(x => x.AccountCode == parentEntity.Code && x.Month == DateTime.Now.Month);
                            //validacion en caso no tenga balance...

                            var log1 = new LogsEntity { }; 

                            if (padrebalance == null)
                            {
                                var newpadrebalance = new BalanceEntity
                                {
                                    Amount = 0,
                                    Month = DateTime.Now.Month,
                                    Year = DateTime.Now.Year,
                                    AccountCode = parentEntity.Code,
                                };

                                _context.Balances.Add(newpadrebalance);
                                await _context.SaveChangesAsync();

                                log1 = new LogsEntity
                                {
                                    table = "balances",
                                    action="post",
                                    oldValue=null,
                                    newValue = JsonConvert.SerializeObject(newpadrebalance)
                                };

                                padrebalance = newpadrebalance;
                            }

                            //Crear partida de padre a hijo...
                            var ParentSonTrns = new JournalEntryEntity
                            {
                                Description = "Partida de transaccion de cuenta a subcuenta..."

                            };

                            _context.JournalEntries.Add(ParentSonTrns);
                            await _context.SaveChangesAsync();
                            
                            var log2 = new LogsEntity
                            {
                                table = "journal_entries",
                                action = "post",
                                oldValue = null,
                                newValue = JsonConvert.SerializeObject(ParentSonTrns)
                            };

                            //crear listado de movimientos....
                            var moves = new List<MovementCreateDto> {

                                 new MovementCreateDto {JournalId = ParentSonTrns.Number, AccountCode = parentEntity.Code, Debit = 0, Credit = padrebalance.Amount },
                                 new MovementCreateDto {JournalId = ParentSonTrns.Number, AccountCode = accountEntity.Code, Debit = padrebalance.Amount , Credit = 0 }

                                };
                            _context.Accounts.Add(accountEntity);
                            await _context.SaveChangesAsync();
                            var log3 = new LogsEntity
                            {
                                table = "accounts",
                                action = "post",
                                oldValue = null,
                                newValue = JsonConvert.SerializeObject(accountEntity)
                            };

                            var balance = new BalanceEntity
                            {
                                Amount = padrebalance.Amount,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                AccountCode = accountEntity.Code,
                            };

                            _context.Balances.Add(balance);
                            await _context.SaveChangesAsync();
                            var log4 = new LogsEntity
                            {
                                table = "balances",
                                action = "post",
                                oldValue = null,
                                newValue = JsonConvert.SerializeObject(balance)
                            };

                            foreach (var item in moves)
                            {
                                var movesentities = _mapper.Map<MovementEntity>(item);
                                _context.Movements.Add(movesentities);
                                await _context.SaveChangesAsync();

                            }


                            var oldparentEntity = parentEntity;
                            parentEntity.AllowMovement = false;
                            _context.Accounts.Update(parentEntity);
                            await _context.SaveChangesAsync();


                            var log5 = new LogsEntity
                            {
                                table = "Accounts",
                                action = "put",
                                oldValue = JsonConvert.SerializeObject(oldparentEntity),
                                newValue = JsonConvert.SerializeObject(parentEntity)
                            };

                            await transaction.CommitAsync();

                            List<LogsEntity> logsList = new List<LogsEntity> {log1,log2,log3,log4,log5 };

                            foreach (var item in logsList)
                            {
                                if(item.newValue != null)
                                {
                                     _logcontext.Logs.Add(item);
                                }
                            }
                            await _logcontext.SaveChangesAsync();


                            var accountDto = _mapper.Map<AccountDto>(accountEntity);
                            return new ResponseDto<AccountDto>
                            {
                                Data = accountDto,
                                StatusCode = 201,
                                Status = true,
                                Message = "Creado exitosamente...",
                            };

                        }

//-------------------------------------------------------------------------------------------------------------------------------------------
                        //En caso de no ser el primer hijo...
                        _context.Accounts.Add(accountEntity);
                        await _context.SaveChangesAsync();
                        var log6 = new LogsEntity
                        {
                            table = "accounts",
                            action = "post",
                            oldValue = null,
                            newValue = JsonConvert.SerializeObject(accountEntity)
                        };



                        //Balance de padre
                        var padreBalance = await _context.Balances.FirstOrDefaultAsync(x => x.AccountCode == parentEntity.Code && x.Month == DateTime.Now.Month);
                        var balances = _context.Balances.Where(x => x.Account.ParentAccountCode == parentEntity.Code).Select(x => x.Amount).ToList();
                        var oldpadrebalance = padreBalance;
                        
                        padreBalance.Amount = balances.Sum();
                        _context.Balances.Update(padreBalance);
                        await _context.SaveChangesAsync();

                        var log7 = new LogsEntity
                        {
                            table = "balances",
                            action = "put",
                            oldValue = JsonConvert.SerializeObject(oldpadrebalance),
                            newValue = JsonConvert.SerializeObject(padreBalance)
                        };

                        await transaction.CommitAsync();
                        List<LogsEntity> logsList2 = new List<LogsEntity> { log6, log7};

                        foreach (var item in logsList2)
                        {
                            if (item.newValue != null)
                            {
                                _logcontext.Logs.Add(item);
                            }
                        }
                        await _logcontext.SaveChangesAsync();

                        var accountdto = _mapper.Map<AccountDto>(accountEntity);
                        return new ResponseDto<AccountDto>
                        {
                            Data = accountdto,
                            StatusCode = 201,
                            Status = true,
                            Message = "Creado exitosamente...",
                        };

                    }

                    //-------------------------------------------------------------------------------------------------------------------------------------
                    //validacion de cuenta sin padre...
                    var accountList = _context.Accounts.Where(x => x.ParentAccountCode == null);
                    accountEntity.Code = accountEntity.ParentAccountCode + (accountList.Count() + 1).ToString();


                    _context.Accounts.Add(accountEntity);
                    await _context.SaveChangesAsync();


                    var log8 = new LogsEntity
                    {
                        table = "accounts",
                        action = "post",
                        oldValue = null,
                        newValue = JsonConvert.SerializeObject(accountEntity)
                    };
                    await transaction.CommitAsync();

                    _logcontext.Logs.Add(log8);
                    await _logcontext.SaveChangesAsync();


                    var Accountdto = _mapper.Map<AccountDto>(accountEntity);
                    return new ResponseDto<AccountDto>
                    {
                        Data = Accountdto,
                        StatusCode = 201,
                        Status = true,
                        Message = "Creado exitosamente...",
                    };

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error al crear cuenta...");
                    return new ResponseDto<AccountDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = "Error al crear la cuenta..."
                    };
                }
            }
        }

        public async Task<ResponseDto<PaginationDto<List<AccountDto>>>> GetAccountsListAsync(string searchTerm = "", int page = 1)
        {
            int startIndex = (page - 1) * PAGE_SIZE;

            var journalEntityQuery = _context.Accounts
                .Where(x => x.CreatedByUser.UserName.Contains(searchTerm.ToLower()) && x.IsBlocked!= true);

            int totalCategories = await journalEntityQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCategories / PAGE_SIZE);


            var accountsEntity = journalEntityQuery
                .OrderBy(u => u.Code)
                .Skip(startIndex)
                .Take(PAGE_SIZE)
                .ToListAsync();


            var accountsDtos = _mapper.Map<List<AccountDto>>(accountsEntity);
           
            var log1 = new LogsEntity
                {
                    table = "accounts",
                    action = "get",
                    oldValue = null,
                    newValue = searchTerm
                };

                _logcontext.Logs.Add(log1);
                await _logcontext.SaveChangesAsync();
            return new ResponseDto<PaginationDto<List<AccountDto>>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Listado de logs obtenida...",
                Data = new PaginationDto<List<AccountDto>>
                {
                    CurrentPage = page,
                    PageSize = PAGE_SIZE,
                    TotalItems = totalCategories,
                    TotalPages = totalPages,
                    Items = accountsDtos,
                    HasPreviousPage = page > 1,
                    HasNextPage = page < totalPages,
                }
            };


        }

        public async Task<ResponseDto<AccountDto>> EditAsync(AccountUpdateDto dto, string code)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var accountEntity = await _context.Accounts.FirstOrDefaultAsync(x => x.Code == code);


                    if (accountEntity == null || accountEntity.IsBlocked == true)
                    {
                        return new ResponseDto<AccountDto>
                        {
                            StatusCode = 404,
                            Status = false,
                            Message = "Cuenta no encontrada..."
                        };
                    }

                    var oldvalue = accountEntity;
                    _mapper.Map<AccountUpdateDto, AccountEntity>(dto, accountEntity);

                    var namecheck = await _context.Accounts.FirstOrDefaultAsync(x => x.Name.Trim() == dto.Name.Trim());
                    if (namecheck != null)
                    {
                        return new ResponseDto<AccountDto>
                        {
                            StatusCode = 403,
                            Status = false,
                            Message = "El nombre ya es utilizado..."
                        };
                    }

                    _context.Accounts.Update(accountEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var log1 = new LogsEntity
                    {
                        table = "accounts",
                        action = "put",
                        oldValue = JsonConvert.SerializeObject(oldvalue),
                        newValue = JsonConvert.SerializeObject(accountEntity)
                    };

                    _logcontext.Logs.Add(log1);
                    await _logcontext.SaveChangesAsync();

                    var accountDto = _mapper.Map<AccountDto>(accountEntity);
                    return new ResponseDto<AccountDto>
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
                    return new ResponseDto<AccountDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = "Error al modificar la cuenta..."
                    };
                }

            }


        }

        public async Task<ResponseDto<AccountDto>> DeleteAsync(string code)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var accountEntity = _context.Accounts.FirstOrDefault(x => x.Code == code);
                    var oldvalue = accountEntity;

                    if (accountEntity == null || accountEntity.IsBlocked == true)
                    {
                        return new ResponseDto<AccountDto>
                        {
                            StatusCode = 404,
                            Status = false,
                            Message = "Cuenta no encontrada..."
                        };
                    }

                    accountEntity.IsBlocked = true;
                    accountEntity.AllowMovement = false;

                    _context.Accounts.Update(accountEntity);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var log1 = new LogsEntity
                    {
                        table = "accounts",
                        action = "delete",
                        oldValue = JsonConvert.SerializeObject(oldvalue),
                        newValue = JsonConvert.SerializeObject(accountEntity)
                    };

                    _logcontext.Logs.Add(log1);
                    await _logcontext.SaveChangesAsync();

                    return new ResponseDto<AccountDto>
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
                    return new ResponseDto<AccountDto>
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
