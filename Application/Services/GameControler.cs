using Core.Exceptions;
using Core.Models;
using DataAccess;
using DataAccess.DbModels;

namespace Application.Services;

public class GameControler
{
    private readonly PlayerRepository _playerRepository;

    private bool _reverseScoring;

    public string GameName { get; private set; }

    public GameControler(PlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;

        GameName = "Game";
    }

    public async Task<IEnumerable<Player>> GetAllPlayers(bool ordered)
    {
        var dbPlayers = await _playerRepository.GetAllPlayers(ordered, _reverseScoring);

        return MappingHelper.MapDbPlayerList(dbPlayers);
    }

    public async Task AddPlayer(Player newPlayer)
    {
        var foundPlayer = await _playerRepository.FindPlayer(newPlayer.Alias);
        if (foundPlayer != null)
            throw new AddPlayerException("Player already exists");

        await _playerRepository.Save(new DbPlayer(newPlayer));
    }

    public async Task RemovePlayer(string playerName)
    {
        var foundPlayer = await _playerRepository.FindPlayer(playerName);

        if (foundPlayer != null)
            await _playerRepository.Remove(foundPlayer.Id);
    }

    public async Task<Player?> AddPlayerScore(string playerName, int newScore)
    {
        var foundPlayer = await _playerRepository.FindPlayer(playerName);
        if (foundPlayer == null)
            return null;

        var player = foundPlayer.ToModel();
        var scoreEntry = new ScoreEntry(newScore);
        player.ScoreEntries.Add(scoreEntry);
        player.TotalScore = GetTotalScore(player);

        await _playerRepository.AddScoreEntry(foundPlayer.Id, new DbScoreEntry(scoreEntry));
        foundPlayer.TotalScore = player.TotalScore;
        await _playerRepository.Update(foundPlayer);

        return player;
    }

    public void SetGameName(string gameName)
    {
        GameName = gameName;
    }

    public void SetReverseScoring(bool reverseScoring)
    {
        _reverseScoring=reverseScoring;
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
