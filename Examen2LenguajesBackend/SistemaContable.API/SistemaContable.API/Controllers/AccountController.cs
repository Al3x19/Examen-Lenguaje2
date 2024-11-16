using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaContable.API.Dtos.Accounts;
using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Services.Interfaces;

namespace SistemaContable.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountController(IAccountsService accountsService) {
            this._accountsService = accountsService;
        }

        [HttpPost]
        //[Authorize(Roles = "User")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Create(AccountCreateDto dto)
        {
            var response = await _accountsService.CreateAsync(dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Update(AccountUpdateDto dto, string code)
        {
            var response = await _accountsService.EditAsync(dto,code);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Delete( string code)
        {
            var response = await _accountsService.DeleteAsync( code);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<AccountDto>>> GetAll(string searchTerm ="", int page = 1)
        {
            var response = await _accountsService.GetAccountsListAsync(searchTerm, page);

            return StatusCode(response.StatusCode, response);
        }

    }
}
