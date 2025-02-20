using Core;
using Core.Exceptions;
using Core.Models;
using DataAccess.DbModels;
using DataAccess.Repositories;

namespace Application.Services;

public class GameControler
{
    private const string DefaultGameName = "Game";

    private readonly PlayerRepository _playerRepository;
    private readonly GameSettingRepository _gameSettingRepository;

    public GameControler(PlayerRepository playerRepository, GameSettingRepository gameSettingRepository)
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
        await _playerRepository.Update(new DbPlayer(renamedPlayer, false));
    }

    private int GetTotalScore(Player p) => p.ScoreEntries.Select(e => e.Value).Sum();

    private async Task<IEnumerable<Player>> GetPlayers(bool ordered)
    {
        var reverseScoring = await GetReverseScoring();
        var dbPlayers = await _playerRepository.GetAllPlayers(ordered, reverseScoring);

        return DBMappingHelper.MapDbPlayerList(dbPlayers);
    }
}
