using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaContable.API.Database;
using SistemaContable.API.Database.Entities;
using SistemaContable.API.Dtos.Accounts;
using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Dtos.JournalEntries;
using SistemaContable.API.Dtos.Logs;
using SistemaContable.API.Services.Interfaces;

namespace SistemaContable.API.Services
{
    public class LogsService : ILogsService
    {
        private readonly LogsContext _context;

        private readonly ILogger<LogsService> _logger;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;

        public LogsService(
            LogsContext context,

            ILogger<LogsService> logger,
            IAuthService authService,
            IConfiguration configuration,
            IMapper mapper
            )
        {
            this._context = context;
            this._logger = logger;
            this._authService = authService;
            this._mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
        }

        public async Task<ResponseDto<PaginationDto<List<AccountsDto>>>> GetLogsListAsync(string searchTerm, int page)
        {
            int startIndex = (page - 1) * PAGE_SIZE;

            var logsQuery = _context.Logs
                .Where(x => x.CreatedBy.Contains(searchTerm.ToLower()));

            int totalCategories = await logsQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCategories / PAGE_SIZE);

            var entriesEntity = await logsQuery
                .OrderBy(u => u.CreatedDate)
                .Skip(startIndex)
                .Take(PAGE_SIZE)
                .ToListAsync();

            var entriesDtos = _mapper.Map<List<AccountsDto>>(entriesEntity);

            return new ResponseDto<PaginationDto<List<AccountsDto>>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Listado de logs obtenida...",
                Data = new PaginationDto<List<AccountsDto>>
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


    }
}
