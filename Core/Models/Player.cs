namespace Core.Models;

public class Player
{
    public int StorageId { get; set; }
    public string Alias { get; set; }
    public List<ScoreEntry> ScoreEntries { get; set; }
    public int TotalScore { get; set; }

    public Player(string alias)
    {
        Alias = alias;

        ScoreEntries = [];
    }
}
