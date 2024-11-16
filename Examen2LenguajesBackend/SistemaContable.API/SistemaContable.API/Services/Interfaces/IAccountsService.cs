using SistemaContable.API.Dtos.Accounts;
using SistemaContable.API.Dtos.Common;

namespace SistemaContable.API.Services.Interfaces
{
    public interface IAccountsService
    {
        Task<ResponseDto<AccountDto>> CreateAsync(AccountCreateDto dto);
        Task<ResponseDto<AccountDto>> DeleteAsync(string code);
        Task<ResponseDto<AccountDto>> EditAsync(AccountUpdateDto dto, string code);
        Task<ResponseDto<PaginationDto<List<AccountDto>>>> GetAccountsListAsync(string searchTerm = "",int page = 1);
    }
}