using Core.Models;

namespace DataAccess.DbModels;

public class DbScoreEntry
{
    public int Id { get; set; }

    public int DbPlayerId { get; set; }
    public DbPlayer DbPlayer { get; set; }

    public int ScoreValue { get; set; }

    public DbScoreEntry()
    {
    }

    public DbScoreEntry(ScoreEntry scoreEntry)
    {
        ScoreValue = scoreEntry.Value;
    }

    public ScoreEntry ToModel() => new ScoreEntry(ScoreValue);
}
