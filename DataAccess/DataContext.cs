using DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext : DbContext
{
    private readonly string _dbFolder;

    public DataContext()
    {
        _dbFolder = ".";
    }

    public DataContext(string dbFolder)
    {
        _dbFolder = dbFolder;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(_dbFolder, "ScoreTracker.db");
        optionsBuilder.UseSqlite($"Filename = {dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbPlayer>()
            .HasMany(p => p.ScoreEntries)
            .WithOne(s => s.Player)
            .HasForeignKey("PlayerId")
            .IsRequired(false);
    }

    public DbSet<DbPlayer> Players { get; set; }
    public DbSet<DbScoreEntry> ScoreEntries { get; set; }
}
