using Microsoft.EntityFrameworkCore;
using ValetaxTest.Domain.Entities;
using ValetaxTest.Infrastructure.Data.Configurations;

namespace ValetaxTest.Infrastructure.Data;

public class ValetaxTestDbContext : DbContext
{
    public ValetaxTestDbContext(DbContextOptions<ValetaxTestDbContext> options) : base(options)
    {
    }

    public DbSet<Node> Nodes { get; set; }
    public DbSet<ExceptionJournal> ExceptionJournals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NodeConfiguration());
        modelBuilder.ApplyConfiguration(new ExceptionJournalConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
