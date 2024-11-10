using PaperScoreTracker.Models;

namespace PaperScoreTracker.Services
{
    public class PlayerControler
    {
        private readonly List<Player> _players;

        public PlayerControler()
        {
            _players = []; //TODO: load from storage
        }

        public async Task<IEnumerable<Player>> GetAllPlayers() => await Task.FromResult((IEnumerable<Player>)_players);

        public void AddPlayer(Player newPlayer)
        {
            _players.Add(newPlayer);
        }

        public void RemovePlayer(string playerName)
        {
            var foundPlayer = FindPlayer(playerName);

            if (foundPlayer != null)
                _players.Remove(foundPlayer);
        }

        public void UpdateScore(string playerName, int newScore)
        {
            var foundPlayer = FindPlayer(playerName);

            if (foundPlayer != null)
                foundPlayer.Score = newScore;
        }

        private Player FindPlayer(string playerName) => _players.FirstOrDefault(p => p.Alias.Equals(playerName));
    }
}
