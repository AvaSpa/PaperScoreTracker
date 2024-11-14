using PaperScoreTracker.Models;

namespace PaperScoreTracker.Services;

public class GameControler
{
    private readonly List<Player> _players;

    public string GameName { get; private set; }

    public GameControler()
    {
        _players = [
        //TEST
            new Player("Player 1"),
            new Player("Player 2"),
            new Player("Player 3"),
            new Player("Player 4"),
            new Player("Player 5"),
            new Player("Player 6")
        //
        ]; //TODO: load from storage

        GameName = "Game";
    }

    public async Task<IEnumerable<Player>> GetAllPlayers() => await Task.FromResult((IEnumerable<Player>)_players.OrderByDescending(p => p.ScoreEntries.Sum()));

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

    public Player? AddPlayerScore(string playerName, int newScore)
    {
        var foundPlayer = FindPlayer(playerName);
        if (foundPlayer == null)
            return null;

        foundPlayer.ScoreEntries.Add(newScore);
        foundPlayer.TotalScore = foundPlayer.ScoreEntries.Sum();

        return foundPlayer;
    }

    public void SetGameName(string gameName)
    {
        GameName = gameName;
    }

    public void ClearPlayers()
    {
        _players.Clear();
    }

    private Player FindPlayer(string playerName) => _players.FirstOrDefault(p => p.Alias.Equals(playerName));
}
