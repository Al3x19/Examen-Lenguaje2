using SistemaContable.API.Dtos.Auth;
using SistemaContable.API.Dtos.Common;

namespace SistemaContable.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);
        Task<ResponseDto<LoginResponseDto>> RegisterAsync(RegisterDto dto);
    }
}