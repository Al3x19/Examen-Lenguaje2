using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaContable.API.Database.Entities;

namespace SistemaContable.API.Database.Configuration
{
    public class JournalConfiguration : IEntityTypeConfiguration<JournalEntryEntity>
    {
        public void Configure(EntityTypeBuilder<JournalEntryEntity> builder)
        {
            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .HasPrincipalKey(e => e.Id);

            builder.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .HasPrincipalKey(e => e.Id);

        }
    }
}