using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValetaxTest.Domain.Entities;

namespace ValetaxTest.Infrastructure.Data.Configurations;

public class ExceptionJournalConfiguration : IEntityTypeConfiguration<ExceptionJournal>
{
    public void Configure(EntityTypeBuilder<ExceptionJournal> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.Property(x => x.Parameters)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.StackTrace)
            .IsRequired()
            .HasColumnType("text");

        builder.HasIndex(x => x.Timestamp);
    }
}