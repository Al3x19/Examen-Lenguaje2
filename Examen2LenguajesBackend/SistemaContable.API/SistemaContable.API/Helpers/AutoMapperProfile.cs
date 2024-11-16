using AutoMapper;
using SistemaContable.API.Database.Entities;
using SistemaContable.API.Dtos.Accounts;
using SistemaContable.API.Dtos.JournalEntries;
using SistemaContable.API.Dtos.Logs;
using SistemaContable.API.Dtos.Movements;

namespace SistemaContable.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForJournalEntries();
            MapsForAccounts();
            MapsForMovements();
            MapsForLogs();
            MapsForMovements();
        
        }
        private void MapsForAccounts()
        {
            CreateMap<AccountEntity, AccountDto>();
            CreateMap<AccountCreateDto, AccountEntity>();
            CreateMap<AccountUpdateDto, AccountEntity>();
        }

        private void MapsForMovements()
        {
            CreateMap<JournalEntryEntity, JournalEntryDto>();
            CreateMap<JournalEntryCreateDto, JournalEntryEntity>();
            CreateMap<JournalEntryUpdateDto, JournalEntryEntity>();
        }

        private void MapsForJournalEntries()
        {
            CreateMap<MovementEntity, MovementDto>();
            CreateMap<MovementCreateDto, MovementEntity>();
            CreateMap<MovementUpdateDto, MovementEntity>();
        }

        private void MapsForLogs()
        {
            CreateMap<LogsEntity, AccountsDto>();

        }

    }
}
