using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Services.Interfaces;
using SistemaContable.API.Dtos.JournalEntries;

namespace SistemaContable.API.Controllers
{
        [ApiController]
        [Route("api/journal")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public class JournalController : ControllerBase
        {
            private readonly IJournalService _journalService;

            public JournalController(IJournalService journalService)
            {
                this._journalService = journalService;
            }

            [HttpPost]
            //[Authorize(Roles = "User")]
            [AllowAnonymous]
            public async Task<ActionResult<ResponseDto<JournalEntryDto>>> Create(JournalEntryCreateDto dto)
            {
                var response = await _journalService.CreateAsync(dto);

                return StatusCode(response.StatusCode, response);
            }

            [HttpPut]
            [AllowAnonymous]
            public async Task<ActionResult<ResponseDto<JournalEntryDto>>> Update(JournalEntryUpdateDto dto,int number)
            {
                var response = await _journalService.UpdateAsync(dto, number);

                return StatusCode(response.StatusCode, response);
            }

            [HttpDelete]
            [AllowAnonymous]
            public async Task<ActionResult<ResponseDto<JournalEntryDto>>> Delete(int number)
            {
                var response = await _journalService.DeleteAsync(number);

                return StatusCode(response.StatusCode, response);
            }

            [HttpGet]
            [AllowAnonymous]
            public async Task<ActionResult<ResponseDto<JournalEntryDto>>> GetAll(string searchTerm = "", int page = 1)
            {
                var response = await _journalService.GetJournalListAsync(searchTerm, page);

                return StatusCode(response.StatusCode, response);
            }

        }
    
}
