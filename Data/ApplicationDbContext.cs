using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    // Construtor que passa as opções para DbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Define a tabela OSModels
    public DbSet<OSModel> OSModels { get; set; }

    // Configurações adicionais ao criar o modelo
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Gera automaticamente o NumeroOS ao adicionar uma nova OS
        modelBuilder.Entity<OSModel>()
            .Property(o => o.NumeroOS)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}