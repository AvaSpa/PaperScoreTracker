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
        Id = scoreEntry.DbId;
        ScoreValue = scoreEntry.Value;
        DbPlayer = new DbPlayer(scoreEntry.Player, false);
        DbPlayerId = DbPlayer.Id;
    }

    public ScoreEntry ToModel() => new ScoreEntry(DbPlayer.ToShallowModel(), ScoreValue)
    {
        DbId = Id
    };
}
