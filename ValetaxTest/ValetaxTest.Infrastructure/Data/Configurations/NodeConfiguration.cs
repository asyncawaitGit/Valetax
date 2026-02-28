using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValetaxTest.Domain.Entities;

namespace ValetaxTest.Infrastructure.Data.Configurations;

public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.TreeName)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(x => x.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.HasIndex(x => x.TreeName);
        builder.HasIndex(x => x.ParentId);

        builder.HasMany(x => x.Children)
            .WithOne()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}