using Core.Models;

namespace DataAccess.Files.FileModels;

public class FileScoreEntry
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int ScoreValue { get; set; }

    public FileScoreEntry()
    {
    }

    public FileScoreEntry(ScoreEntry scoreEntry)
    {
        Id = scoreEntry.StorageId;
        ScoreValue = scoreEntry.Value;
        PlayerId = scoreEntry.Player.StorageId;
    }

    public ScoreEntry ToModel(Player player)
    {
        var model = new ScoreEntry(player, ScoreValue)
        {
            StorageId = Id
        };

        return model;
    }
}
