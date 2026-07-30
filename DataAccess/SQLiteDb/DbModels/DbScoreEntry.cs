using Core.Models;

namespace DataAccess.SQLiteDb.DbModels;

public class DbScoreEntry
{
    public int Id { get; set; }

    public int DbPlayerId { get; set; }

    public DbPlayer? DbPlayer { get; set; }

    public int ScoreValue { get; set; }

    public DbScoreEntry()
    {
    }

    public DbScoreEntry(ScoreEntry scoreEntry)
    {
        Id = scoreEntry.StorageId;
        ScoreValue = scoreEntry.Value;
        DbPlayerId = scoreEntry.Player?.StorageId ?? 0;
        DbPlayer = null;
    }

    public ScoreEntry ToModel()
    {
        var playerModel = DbPlayer != null
            ? DbPlayer.ToShallowModel()
            : new Player(string.Empty) { StorageId = DbPlayerId };

        return new ScoreEntry(playerModel, ScoreValue)
        {
            StorageId = Id
        };
    }
}