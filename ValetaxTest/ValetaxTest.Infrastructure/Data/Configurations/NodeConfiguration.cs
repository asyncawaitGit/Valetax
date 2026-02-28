using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValetaxTest.Domain.Entities;

namespace ValetaxTest.Infrastructure.Data.Configurations;

public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.HasIndex(x => x.TreeId);
        builder.HasIndex(x => x.ParentId);

        builder.HasMany(x => x.Children)
            .WithOne()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.Children);

        builder.Property(x => x.ParentId)
            .IsRequired(false);
    }
}