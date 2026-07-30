using Core.Interfaces;
using Core.Models;
using DataAccess.Files.FileModels;
using System.Text.Json;

namespace DataAccess.Files.Repositories;

public class FilePlayerRepository : FileBaseRepository, IPlayerRepository
{
    private const string FileName = "players.json";

    public FilePlayerRepository(string filesFolder) : base(filesFolder)
    {
    }

    private async Task<List<FilePlayer>> ReadPlayers()
    {
        var path = GetPath(FileName);
        var text = await File.ReadAllTextAsync(path);
        var items = JsonSerializer.Deserialize<List<FilePlayer>>(text, _serializerOptions) ?? new List<FilePlayer>();
        return items;
    }

    private async Task WritePlayers(List<FilePlayer> players)
    {
        var path = GetPath(FileName);
        var outText = JsonSerializer.Serialize(players, _serializerOptions);
        await File.WriteAllTextAsync(path, outText);
    }

    public async Task<FilePlayer?> FindPlayer(int playerId)
    {
        var players = await ReadPlayers();
        return players.FirstOrDefault(p => p.Id == playerId);
    }

    public async Task<FilePlayer?> FindPlayer(string playerName)
    {
        var players = await ReadPlayers();
        return players.FirstOrDefault(p => p.Alias == playerName);
    }

    public async Task<FilePlayer?> FindPlayerByScoreEntryId(int scoreEntryId)
    {
        var players = await ReadPlayers();
        return players.FirstOrDefault(p => p.ScoreEntries.Any(se => se.Id == scoreEntryId));
    }

    public async Task Save(FilePlayer newPlayer)
    {
        var players = await ReadPlayers();
        var id = players.Any() ? players.Max(p => p.Id) + 1 : 1;
        newPlayer.Id = newPlayer.Id == 0 ? id : newPlayer.Id;

        // assign ids for score entries if not set
        if (newPlayer.ScoreEntries != null && newPlayer.ScoreEntries.Any())
        {
            var nextEntryId = players.SelectMany(p => p.ScoreEntries).DefaultIfEmpty().Max(se => se == null ? 0 : se.Id) + 1;
            foreach (var se in newPlayer.ScoreEntries)
            {
                if (se.Id == 0)
                    se.Id = nextEntryId++;
                se.PlayerId = newPlayer.Id;
            }
        }

        // recalc total
        newPlayer.TotalScore = newPlayer.ScoreEntries?.Sum(se => se.ScoreValue) ?? 0;

        players.Add(newPlayer);
        await WritePlayers(players);
    }

    public async Task<IEnumerable<FilePlayer>> GetAllPlayers(bool ordered, bool _reverseScoring)
    {
        var players = await ReadPlayers();
        if (!ordered) return players;

        return _reverseScoring
            ? players.OrderBy(p => p.TotalScore).ToList()
            : players.OrderByDescending(p => p.TotalScore).ToList();
    }

    public async Task Remove(int playerId)
    {
        var players = await ReadPlayers();
        var found = players.FirstOrDefault(p => p.Id == playerId);
        if (found == null) return;

        players.Remove(found);
        await WritePlayers(players);
    }

    public async Task Clear()
    {
        await WritePlayers(new List<FilePlayer>());
    }

    public async Task ClearScores()
    {
        var players = await ReadPlayers();
        foreach (var p in players)
        {
            p.TotalScore = 0;
            p.ScoreEntries.Clear();
        }

        await WritePlayers(players);
    }

    public async Task Update(FilePlayer filePlayer)
    {
        var players = await ReadPlayers();
        var idx = players.FindIndex(p => p.Id == filePlayer.Id);
        if (idx < 0) return;

        // ensure score entries have PlayerId
        if (filePlayer.ScoreEntries != null)
        {
            foreach (var se in filePlayer.ScoreEntries)
                se.PlayerId = filePlayer.Id;

            filePlayer.TotalScore = filePlayer.ScoreEntries.Sum(se => se.ScoreValue);
        }

        players[idx] = filePlayer;
        await WritePlayers(players);
    }

    public async Task UpdateScoreEntry(FileScoreEntry fileScoreEntry)
    {
        var players = await ReadPlayers();
        var player = players.FirstOrDefault(p => p.ScoreEntries.Any(se => se.Id == fileScoreEntry.Id));
        if (player == null) return;

        var idx = player.ScoreEntries.FindIndex(se => se.Id == fileScoreEntry.Id);
        if (idx < 0) return;

        player.ScoreEntries[idx] = fileScoreEntry;
        player.TotalScore = player.ScoreEntries.Sum(se => se.ScoreValue);

        await WritePlayers(players);
    }

    public async Task AddScoreEntry(int playerId, FileScoreEntry scoreEntry)
    {
        var players = await ReadPlayers();
        var player = players.FirstOrDefault(p => p.Id == playerId);
        if (player == null) return;

        var nextEntryId = players.SelectMany(p => p.ScoreEntries).DefaultIfEmpty().Max(se => se == null ? 0 : se.Id) + 1;
        scoreEntry.Id = scoreEntry.Id == 0 ? nextEntryId : scoreEntry.Id;
        scoreEntry.PlayerId = playerId;

        player.ScoreEntries.Add(scoreEntry);
        player.TotalScore = player.ScoreEntries.Sum(se => se.ScoreValue);

        await WritePlayers(players);
    }

    public async Task DeleteScoreEntry(int scoreEntryId)
    {
        var players = await ReadPlayers();
        var player = players.FirstOrDefault(p => p.ScoreEntries.Any(se => se.Id == scoreEntryId));
        if (player == null) return;

        var se = player.ScoreEntries.FirstOrDefault(x => x.Id == scoreEntryId);
        if (se != null) player.ScoreEntries.Remove(se);

        player.TotalScore = player.ScoreEntries.Sum(s => s.ScoreValue);

        await WritePlayers(players);
    }

    public async Task<int> CountPlayers()
    {
        var players = await ReadPlayers();
        return players.Count;
    }

    public async Task SeedPlayers()
    {
        var p1 = new FilePlayer { Alias = "Player 1" };
        var p2 = new FilePlayer { Alias = "Player 2" };

        await Save(p1);
        await Save(p2);
    }

    public async Task<Player?> GetPlayerById(int playerId)
    {
        var fp = await FindPlayer(playerId);
        return fp?.ToModel();
    }

    public async Task<Player?> GetPlayerByName(string playerName)
    {
        var fp = await FindPlayer(playerName);
        return fp?.ToModel();
    }

    public async Task<Player?> GetPlayerByScoreEntryId(int scoreEntryId)
    {
        var fp = await FindPlayerByScoreEntryId(scoreEntryId);
        return fp?.ToModel();
    }

    public async Task SavePlayer(Player newPlayer)
    {
        await Save(new FilePlayer(newPlayer, true));
    }

    public async Task<IEnumerable<Player>> GetAllPlayerModels(bool ordered, bool reverseScoring)
    {
        var filePlayers = await GetAllPlayers(ordered, reverseScoring);
        return filePlayers.Select(fp => fp.ToModel());
    }

    public async Task RemovePlayer(int playerId)
    {
        await Remove(playerId);
    }

    public async Task UpdatePlayer(Player player)
    {
        await Update(new FilePlayer(player, true));
    }

    public async Task UpdateScoreEntry(ScoreEntry scoreEntry)
    {
        await UpdateScoreEntry(new FileScoreEntry(scoreEntry));
    }

    public async Task AddScoreEntry(int playerId, ScoreEntry scoreEntry)
    {
        await AddScoreEntry(playerId, new FileScoreEntry(scoreEntry));
    }
}