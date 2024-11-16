using Core.Models;
using DataAccess;

namespace Application.Services;

public class GameControler
{
    private readonly List<Player> _players;
    private readonly PlayerRepository _playerRepository;

    public string GameName { get; private set; }

    public GameControler(PlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;

        GameName = "Game";
    }

    public async Task<IEnumerable<Player>> GetAllPlayers()
    {
        var dbPlayers = await _playerRepository.GetAllPlayers();

        return MappingHelper.MapDbPlayerList(dbPlayers);
    }

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

        foundPlayer.ScoreEntries.Add(new ScoreEntry(newScore));
        foundPlayer.TotalScore = GetTotalScore(foundPlayer);

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

    private Player? FindPlayer(string playerName) => _players.FirstOrDefault(p => p.Alias.Equals(playerName));

    private int GetTotalScore(Player p) => p.ScoreEntries.Select(e => e.Value).Sum();
}
