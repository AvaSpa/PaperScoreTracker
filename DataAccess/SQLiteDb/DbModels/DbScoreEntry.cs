using Core.Models;

namespace DataAccess.SQLiteDb.DbModels;

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
        Id = scoreEntry.StorageId;
        ScoreValue = scoreEntry.Value;
        DbPlayer = new DbPlayer(scoreEntry.Player, false);
        DbPlayerId = DbPlayer.Id;
    }

    public ScoreEntry ToModel() => new ScoreEntry(DbPlayer.ToShallowModel(), ScoreValue)
    {
        StorageId = Id
    };
}
