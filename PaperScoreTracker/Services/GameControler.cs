using PaperScoreTracker.Models;

namespace PaperScoreTracker.Services
{
    public class GameControler
    {
        private readonly List<Player> _players;

        public string GameName { get; private set; }

        public GameControler()
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

        public void SetGameName(string gameName)
        {
            GameName = gameName;
        }

        private Player FindPlayer(string playerName) => _players.FirstOrDefault(p => p.Alias.Equals(playerName));
    }
}
