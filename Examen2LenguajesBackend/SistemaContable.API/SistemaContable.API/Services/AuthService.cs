
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SistemaContable.API.Database;
using SistemaContable.API.Database.Entities;
using SistemaContable.API.Dtos.Auth;
using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SistemaContable.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly LogsContext _logsContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            SignInManager<UserEntity> signInManager,
            LogsContext logsContext,
            UserManager<UserEntity> userManager, 
            IConfiguration configuration
            )
        {
            this._signInManager = signInManager;
            this._logsContext = logsContext;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            var result = await _signInManager
                .PasswordSignInAsync(dto.Email, 
                                     dto.Password, 
                                     isPersistent: false, 
                                     lockoutOnFailure: false);

            if(result.Succeeded) 
            {
                var userEntity = await _userManager.FindByEmailAsync(dto.Email);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userEntity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", userEntity.Id),
                };

                var userRoles = await _userManager.GetRolesAsync(userEntity);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtToken = GetToken(authClaims);

                var log1 = new LogsEntity
                {
                    table = null,
                    action = "Log in",
                    oldValue = null,
                    newValue = JsonConvert.SerializeObject(dto.Email)
                };

                _logsContext.Logs.Add(log1);
                await _logsContext.SaveChangesAsync();
                return new ResponseDto<LoginResponseDto> 
                {
                    StatusCode = 200,
                    Status = true,
                    Message = "Inicio de sesion satisfactorio",
                    Data = new LoginResponseDto 
                    {
                        Email = userEntity.Email,
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        TokenExpiration = jwtToken.ValidTo,
                    }
                };

            }

            return new ResponseDto<LoginResponseDto> 
            {
                Status = false,
                StatusCode = 401,
                Message = "Fallo el inicio de sesión"
            };


        }

        public async Task<ResponseDto<LoginResponseDto>> RegisterAsync(RegisterDto dto) 
        {
            var user = new UserEntity 
            {
                UserName = dto.Email,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded) 
            {
                var userEntity = await _userManager.FindByEmailAsync(dto.Email);

                await _userManager.AddToRoleAsync(userEntity,"User");

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userEntity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", userEntity.Id),
                    new Claim(ClaimTypes.Role, "User")
                };

                var jwtToken = GetToken(authClaims);

                return new ResponseDto<LoginResponseDto> 
                {
                    StatusCode = 200,
                    Status = true,
                    Message = "Registro de usuario realizado satisfactoriamente.",
                    Data = new LoginResponseDto 
                    {
                        Email = userEntity.Email,
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        TokenExpiration = jwtToken.ValidTo,
                    }
                };
            }

            return new ResponseDto<LoginResponseDto> 
            {
                StatusCode = 400,
                Status = false,
                Message = "Error al registrar el usuario"
            };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration["JWT:Secret"]));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:Expires"]??"15")),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, 
                    SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
