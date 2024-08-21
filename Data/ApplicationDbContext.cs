using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<OSModel> OSModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OSModel>()
            .Property(o => o.NumeroOS)
            .ValueGeneratedOnAdd(); // Configura o campo para ser gerado automaticamente

        base.OnModelCreating(modelBuilder);
    }
}