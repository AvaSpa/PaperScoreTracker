using Core.Models;

namespace DataAccess.SQLiteDb.DbModels;

public class DbPlayer
{
    public int Id { get; set; }

    public string Alias { get; set; }

    public int TotalScore { get; set; }

    public ICollection<DbScoreEntry> DbScoreEntries { get; set; }

    public DbPlayer()
    {
        DbScoreEntries = [];
    }

    public DbPlayer(Player player, bool includeScoreEntries = true) : this()
    {
        Id = player.StorageId;
        Alias = player.Alias;
        TotalScore = player.TotalScore;

        if (includeScoreEntries)
            DbScoreEntries = [.. player.ScoreEntries.Select(e => new DbScoreEntry(e))];
    }

    public Player ToModel() => new(Alias)
    {
        StorageId = Id,
        TotalScore = TotalScore,
        ScoreEntries = [.. DbScoreEntries.Select(e => e.ToModel())]
    };

    public Player ToShallowModel() => new(Alias)
    {
        StorageId = Id,
        TotalScore = TotalScore,
        ScoreEntries = []
    };
}
