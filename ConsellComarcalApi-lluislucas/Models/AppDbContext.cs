namespace ConsellComarcalApi.Models;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Edificacio> Edificacions { get; set; }
    public DbSet<Contribuent> Contribuents { get; set; }

    public DbSet<Poble> Pobles { get; set; }

    public DbSet<Tipus> Tipologies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = "Server=localhost;Database=comarcal;User=root;Password=president;";
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}