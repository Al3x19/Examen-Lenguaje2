using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Dtos.Logs;

namespace SistemaContable.API.Services.Interfaces
{
    public interface ILogsService
    {
        Task<ResponseDto<PaginationDto<List<AccountsDto>>>> GetLogsListAsync(string searchTerm, int page);
    }
}