using Core.Models;

namespace DataAccess.Files.FileModels;

public class FilePlayer
{
    public int Id { get; set; }

    public string Alias { get; set; }

    public int TotalScore { get; set; }

    public List<FileScoreEntry> ScoreEntries { get; set; }

    public FilePlayer()
    {
        ScoreEntries = new List<FileScoreEntry>();
    }

    public FilePlayer(Player player, bool includeScoreEntries = true) : this()
    {
        Id = player.StorageId;
        Alias = player.Alias;
        TotalScore = player.TotalScore;

        if (includeScoreEntries && player.ScoreEntries != null)
            ScoreEntries = player.ScoreEntries.Select(e => new FileScoreEntry(e)).ToList();
    }

    public Player ToModel()
    {
        var p = new Player(Alias)
        {
            StorageId = Id,
            TotalScore = TotalScore,
            ScoreEntries = ScoreEntries?.Select(se => se.ToModel(new Player(Alias) { StorageId = Id })).ToList() ?? new List<ScoreEntry>()
        };

        // Fix circular references: assign player instance to each score entry
        foreach (var se in p.ScoreEntries)
            se.Player = p;

        return p;
    }

    public Player ToShallowModel() => new(Alias)
    {
        StorageId = Id,
        TotalScore = TotalScore,
        ScoreEntries = new List<ScoreEntry>()
    };
}
