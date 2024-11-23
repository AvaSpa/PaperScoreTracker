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

    public DbPlayer(Player player) : this()
    {
        Alias = player.Alias;
        TotalScore = player.TotalScore;

        DbScoreEntries = [.. player.ScoreEntries.Select(e => new DbScoreEntry(e))];
    }

    public Player ToModel() => new(Alias)
    {
        TotalScore = TotalScore,
        ScoreEntries = [.. DbScoreEntries.Select(e => e.ToModel())]
    };
}
