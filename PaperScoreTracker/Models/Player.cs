namespace PaperScoreTracker.Models
{
    public class Player
    {
        public string Alias { get; set; }
        public int Score { get; set; }

        public Player(string alias)
        {
            Alias = alias;
        }
    }
}
