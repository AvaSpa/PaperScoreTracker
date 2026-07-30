using Core;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;

namespace Application.Services;

public class GameControler
{
    private const string DefaultGameName = "Game";

    private readonly IPlayerRepository _playerRepository;
    private readonly IGameSettingRepository _gameSettingRepository;

    public GameControler(IPlayerRepository playerRepository, IGameSettingRepository gameSettingRepository)
    {
        _playerRepository = playerRepository;
        _gameSettingRepository = gameSettingRepository;
    }

    public async Task<IEnumerable<Player>> GetAllPlayers(bool ordered)
    {
        var players = await GetPlayers(ordered);

        if (players.Any())
            return players;

        await _playerRepository.SeedPlayers();

        return await GetPlayers(ordered);
    }

    public async Task AddPlayer(Player newPlayer)
    {
        var foundPlayer = await _playerRepository.GetPlayerByName(newPlayer.Alias);
        if (foundPlayer != null)
            throw new AddPlayerException("Player already exists");

        await _playerRepository.SavePlayer(newPlayer);
    }

    public async Task RemovePlayer(string playerName)
    {
        var foundPlayer = await _playerRepository.GetPlayerByName(playerName);

        if (foundPlayer != null)
            await _playerRepository.RemovePlayer(foundPlayer.StorageId);
    }

    public async Task<Player?> AddPlayerScore(string playerName, int newScore)
    {
        var foundPlayer = await _playerRepository.GetPlayerByName(playerName);
        if (foundPlayer == null)
            return null;

        var scoreEntry = new ScoreEntry(foundPlayer, newScore);
        foundPlayer.ScoreEntries.Add(scoreEntry);
        foundPlayer.TotalScore = GetTotalScore(foundPlayer);

        await _playerRepository.AddScoreEntry(foundPlayer.StorageId, scoreEntry);

        await UpdateTotalScore(foundPlayer, foundPlayer.TotalScore);

        return foundPlayer;
    }

    public async Task<string> GetGameName()
    {
        var gameName = await _gameSettingRepository.GetGameSettingValue(SettingName.GameName);

        return string.IsNullOrEmpty(gameName) ? DefaultGameName : gameName;
    }

    public async Task SetGameName(string gameName)
    {
        var gameNameSetting = new GameSetting(SettingName.GameName, gameName);

        await _gameSettingRepository.SaveGameSetting(gameNameSetting);
    }

    public async Task<bool> GetReverseScoring()
    {
        var reverseScoringValue = await _gameSettingRepository.GetGameSettingValue(SettingName.ReverseScoring);

        return string.IsNullOrEmpty(reverseScoringValue)
            ? false
            : Convert.ToBoolean(reverseScoringValue);
    }

    public async Task SetReverseScoring(bool reverseScoring)
    {
        var reverseScoringSetting = new GameSetting(SettingName.ReverseScoring, reverseScoring.ToString());

        await _gameSettingRepository.SaveGameSetting(reverseScoringSetting);
    }

    public async Task ClearPlayers()
    {
        await _playerRepository.Clear();
    }

    public async Task ClearScores()
    {
        await _playerRepository.ClearScores();
    }

    public async Task<int> GetPlayerCount()
    {
        return await _playerRepository.CountPlayers();
    }

    public async Task UpdateAlias(Player renamedPlayer)
    {
        await _playerRepository.UpdatePlayer(renamedPlayer);
    }

    public async Task UpdateScoreEntry(ScoreEntry updatedScoreEntry)
    {
        await _playerRepository.UpdateScoreEntry(updatedScoreEntry);

        var foundPlayer = await _playerRepository.GetPlayerByName(updatedScoreEntry.Player.Alias);
        if (foundPlayer == null)
            return;

        await UpdateTotalScore(foundPlayer, GetTotalScore(foundPlayer));
    }

    public async Task DeleteScoreEntry(int scoreEntryId)
    {
        var foundPlayer = await _playerRepository.GetPlayerByScoreEntryId(scoreEntryId);
        if (foundPlayer == null)
            return;

        await _playerRepository.DeleteScoreEntry(scoreEntryId);

        var updatedPlayer = await _playerRepository.GetPlayerById(foundPlayer.StorageId);
        if (updatedPlayer == null)
            return;

        await UpdateTotalScore(updatedPlayer, GetTotalScore(updatedPlayer));
    }

    private async Task UpdateTotalScore(Player foundPlayer, int newTotalScore)
    {
        foundPlayer.TotalScore = newTotalScore;
        await _playerRepository.UpdatePlayer(foundPlayer);
    }

    private int GetTotalScore(Player p) => p.ScoreEntries.Select(e => e.Value).Sum();

    private async Task<IEnumerable<Player>> GetPlayers(bool ordered)
    {
        var reverseScoring = await GetReverseScoring();
        var players = await _playerRepository.GetAllPlayerModels(ordered, reverseScoring);

        return players;
    }
}
