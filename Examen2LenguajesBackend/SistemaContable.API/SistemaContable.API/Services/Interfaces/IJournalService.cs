using SistemaContable.API.Dtos.Common;
using SistemaContable.API.Dtos.JournalEntries;

namespace SistemaContable.API.Services.Interfaces
{
    public interface IJournalService
    {
        Task<ResponseDto<JournalEntryDto>> CreateAsync(JournalEntryCreateDto dto);
        Task<ResponseDto<JournalEntryDto>> DeleteAsync(int number);
        Task<ResponseDto<PaginationDto<List<JournalEntryDto>>>> GetJournalListAsync(string searchTerm = "", int page = 1);
        Task<ResponseDto<JournalEntryDto>> UpdateAsync(JournalEntryUpdateDto dto, int number);
    }
}