using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Dtos.Logs;
using SistemaContable.API.Services.Interfaces;

namespace SistemaContable.API.Controllers
{
    [ApiController]
    [Route("api/logs")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LogsController : ControllerBase
    {
        private readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            this._logsService = logsService;
        }



        [HttpGet]
        //[Authorize(Roles = "User")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<AccountsDto>>> GetAll(string searchTerm = "", int page = 1)
        {
            var response = await _logsService.GetLogsListAsync(searchTerm, page);

            return StatusCode(response.StatusCode, response);
        }

    }

}



