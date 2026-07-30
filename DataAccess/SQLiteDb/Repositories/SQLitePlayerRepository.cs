using Core.Interfaces;
using Core.Models;
using DataAccess.SQLiteDb.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQLiteDb.Repositories;

public class SQLitePlayerRepository : SQLiteBaseRepository, IPlayerRepository
{
    public SQLitePlayerRepository(string dbFolder) : base(dbFolder)
    {
    }

    // DB model methods
    public async Task<DbPlayer?> FindPlayer(int playerId)
    {
        using var ctx = new DataContext(_dbFolder);

        return await ctx.Players.FindAsync(playerId);
    }

    public async Task<DbPlayer?> FindPlayer(string playerName)
    {
        using var ctx = new DataContext(_dbFolder);

        return await ctx.Players.FirstOrDefaultAsync(p => p.Alias == playerName);
    }

    public async Task<DbPlayer?> FindPlayerByScoreEntryId(int scoreEntryId)
    {
        using var ctx = new DataContext(_dbFolder);

        var foundScoreEntry = await ctx.ScoreEntries.FindAsync(scoreEntryId);
        if (foundScoreEntry == null)
            return null;

        return foundScoreEntry.DbPlayer;
    }

    public async Task Save(DbPlayer newPlayer)
    {
        using var ctx = new DataContext(_dbFolder);

        await ctx.Players.AddAsync(newPlayer);
        await ctx.SaveChangesAsync();
    }

    public async Task<IEnumerable<DbPlayer>> GetAllPlayers(bool ordered, bool _reverseScoring)
    {
        using var ctx = new DataContext(_dbFolder);

        return ordered
            ? _reverseScoring
                ? await ctx.Players.OrderBy(p => p.TotalScore).ToListAsync()
                : await ctx.Players.OrderByDescending(p => p.TotalScore).ToListAsync()
            : await ctx.Players.ToListAsync();
    }

    public async Task Remove(int playerId)
    {
        using var ctx = new DataContext(_dbFolder);

        var foundPlayer = await ctx.Players.FindAsync(playerId);
        if (foundPlayer == null)
            return;

        ctx.Players.Remove(foundPlayer);
        await ctx.SaveChangesAsync();
    }

    public async Task Clear()
    {
        using var ctx = new DataContext(_dbFolder);

        ctx.Players.RemoveRange(ctx.Players);
        await ctx.SaveChangesAsync();
    }

    public async Task ClearScores()
    {
        using var ctx = new DataContext(_dbFolder);

        foreach (var player in ctx.Players)
            player.TotalScore = 0;

        ctx.ScoreEntries.RemoveRange(ctx.ScoreEntries);
        await ctx.SaveChangesAsync();
    }

    public async Task Update(DbPlayer dbPlayer)
    {
        using var ctx = new DataContext(_dbFolder);

        var existing = await ctx.Players.FindAsync(dbPlayer.Id);
        if (existing == null)
            return;

        existing.Alias = dbPlayer.Alias;
        existing.TotalScore = dbPlayer.TotalScore;

        await ctx.SaveChangesAsync();
    }

    public async Task UpdateScoreEntry(DbScoreEntry dbScoreEntry)
    {
        using var ctx = new DataContext(_dbFolder);

        ctx.ScoreEntries.Update(dbScoreEntry);
        await ctx.SaveChangesAsync();
    }

    public async Task AddScoreEntry(int playerId, DbScoreEntry scoreEntry)
    {
        using var ctx = new DataContext(_dbFolder);

        var foundPlayer = await ctx.Players.FindAsync(playerId);
        if (foundPlayer == null)
            return;

        scoreEntry.DbPlayer = foundPlayer;
        scoreEntry.DbPlayerId = foundPlayer.Id;

        foundPlayer.DbScoreEntries.Add(scoreEntry);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteScoreEntry(int scoreEntryId)
    {
        using var ctx = new DataContext(_dbFolder);

        var foundScoreEntry = await ctx.ScoreEntries.FindAsync(scoreEntryId);
        if (foundScoreEntry == null)
            return;

        ctx.ScoreEntries.Remove(foundScoreEntry);
        await ctx.SaveChangesAsync();
    }

    public async Task<int> CountPlayers()
    {
        using var ctx = new DataContext(_dbFolder);

        return await ctx.Players.CountAsync();
    }

    public async Task SeedPlayers()
    {
        var player1 = new DbPlayer() { Alias = "Player 1" };
        var player2 = new DbPlayer() { Alias = "Player 2" };

        await Save(player1);
        await Save(player2);
    }

    // Interface wrappers (work with Core.Models)
    public async Task<Player?> GetPlayerById(int playerId)
    {
        var db = await FindPlayer(playerId);
        return db?.ToModel();
    }

    public async Task<Player?> GetPlayerByName(string playerName)
    {
        var db = await FindPlayer(playerName);
        return db?.ToModel();
    }

    public async Task<Player?> GetPlayerByScoreEntryId(int scoreEntryId)
    {
        var db = await FindPlayerByScoreEntryId(scoreEntryId);
        return db?.ToModel();
    }

    public async Task SavePlayer(Player newPlayer)
    {
        await Save(new DbPlayer(newPlayer));
    }

    public async Task<IEnumerable<Player>> GetAllPlayerModels(bool ordered, bool reverseScoring)
    {
        var dbPlayers = await GetAllPlayers(ordered, reverseScoring);
        return dbPlayers.Select(dp => dp.ToModel());
    }

    public async Task RemovePlayer(int playerId)
    {
        await Remove(playerId);
    }

    public async Task UpdatePlayer(Player player)
    {
        await Update(new DbPlayer(player, true));
    }

    public async Task UpdateScoreEntry(ScoreEntry scoreEntry)
    {
        await UpdateScoreEntry(new DbScoreEntry(scoreEntry));
    }

    public async Task AddScoreEntry(int playerId, ScoreEntry scoreEntry)
    {
        await AddScoreEntry(playerId, new DbScoreEntry(scoreEntry));
    }

    // DeleteScoreEntry, CountPlayers and SeedPlayers already match interface names
}
