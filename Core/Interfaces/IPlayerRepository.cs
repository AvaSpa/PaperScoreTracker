using Core.Models;

namespace Core.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerById(int playerId);
    Task<Player?> GetPlayerByName(string playerName);
    Task<Player?> GetPlayerByScoreEntryId(int scoreEntryId);
    Task SavePlayer(Player newPlayer);
    Task<IEnumerable<Player>> GetAllPlayerModels(bool ordered, bool reverseScoring);
    Task RemovePlayer(int playerId);
    Task Clear();
    Task ClearScores();
    Task UpdatePlayer(Player player);
    Task UpdateScoreEntry(ScoreEntry scoreEntry);
    Task AddScoreEntry(int playerId, ScoreEntry scoreEntry);
    Task DeleteScoreEntry(int scoreEntryId);
    Task<int> CountPlayers();
    Task SeedPlayers();
}
