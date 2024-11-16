using DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext : DbContext
{
    public DataContext()
    {
        //TODO: add foreign keys (and support for sqlite) and the necessary properties to the db model classes
        //see the whole video: https://youtu.be/lTCVZ6x8-Io?t=1643
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ScoreTracker.db");
    }

    public DbSet<DbPlayer> Players { get; set; }
    public DbSet<DbScoreEntry> ScoreEntries { get; set; }
}
