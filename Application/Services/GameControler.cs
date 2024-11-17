using Core.Models;
using DataAccess;
using DataAccess.DbModels;

namespace Application.Services;

public class GameControler
{
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

    public async Task AddPlayer(Player newPlayer)
    {
        await _playerRepository.Save(new DbPlayer(newPlayer));
    }

    public async Task RemovePlayer(string playerName)
    {
        var foundPlayer = _playerRepository.FindPlayer(playerName);

        if (foundPlayer != null)
            await _playerRepository.Remove(foundPlayer.Id);
    }

    public async Task<Player?> AddPlayerScore(string playerName, int newScore)
    {
        var foundPlayer = await _playerRepository.FindPlayer(playerName);
        if (foundPlayer == null)
            return null;

        var player = foundPlayer.ToModel();
        player.ScoreEntries.Add(new ScoreEntry(newScore));
        player.TotalScore = GetTotalScore(player);

        await _playerRepository.Update(new DbPlayer(player));

        return player;
    }

    public void SetGameName(string gameName)
    {
        GameName = gameName;
    }

    public async Task ClearPlayers()
    {
        await _playerRepository.Clear();
    }

    public async Task<int> GetPlayerCount()
    {
        return await _playerRepository.CountPlayers();
    }

    private int GetTotalScore(Player p) => p.ScoreEntries.Select(e => e.Value).Sum();
}
