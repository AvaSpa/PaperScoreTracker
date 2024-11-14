namespace PaperScoreTracker.Models;

public class Player
{
    public string Alias { get; set; }
    public IList<int> ScoreEntries { get; set; }
    public int TotalScore { get; set; }

    public Player(string alias)
    {
        Alias = alias;

        ScoreEntries = [];
    }
}
