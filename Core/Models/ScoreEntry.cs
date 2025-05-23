namespace Core.Models;

public class ScoreEntry
{
    public int DbId { get; set; }

    public int Value { get; set; }

    public Player Player { get; set; }

    public ScoreEntry(Player player, int value)
    {
        Player = player;
        Value = value;
    }
}
