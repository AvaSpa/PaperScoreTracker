using DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class PlayerRepository : BaseRepository
{
    public PlayerRepository(string dbFolder) : base(dbFolder)
    {
    }

    public async Task<DbPlayer?> FindPlayer(string playerName)
    {
        using var ctx = new DataContext(_dbFolder);

        return await ctx.Players.FirstOrDefaultAsync(p => p.Alias == playerName);
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

        ctx.Players.Update(dbPlayer);
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

        foundPlayer.DbScoreEntries.Add(scoreEntry);
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
}
