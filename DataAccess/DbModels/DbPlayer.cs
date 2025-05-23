using Core.Models;

namespace DataAccess.DbModels;

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
        Id = player.DbId;
        Alias = player.Alias;
        TotalScore = player.TotalScore;

        if (includeScoreEntries)
            DbScoreEntries = [.. player.ScoreEntries.Select(e => new DbScoreEntry(e))];
    }

    public Player ToModel() => new(Alias)
    {
        DbId = Id,
        TotalScore = TotalScore,
        ScoreEntries = [.. DbScoreEntries.Select(e => e.ToModel())]
    };

    public Player ToShallowModel() => new(Alias)
    {
        DbId = Id,
        TotalScore = TotalScore,
        ScoreEntries = []
    };
}
