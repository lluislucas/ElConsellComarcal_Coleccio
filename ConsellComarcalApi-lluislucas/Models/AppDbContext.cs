namespace ConsellComarcalApi.Models;

public class AppDbContext : DbContext
{
    public DbSet<Edificacio> Edificacions { get; set; }
    public DbSet<Contribuent> Contribuents { get; set; }

    public DbSet<Poble> Pobles { get; set; }

    public DbSet<Tipus> Tipologies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=localhost;Database=MiDB;Trusted_Connection=true;");
    }
}