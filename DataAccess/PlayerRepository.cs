using DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class PlayerRepository
{
    private readonly string _dbFolder;

    public PlayerRepository(string dbFolder)
    {
        _dbFolder = dbFolder;
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

    public async Task<IEnumerable<DbPlayer>> GetAllPlayers()
    {
        using var ctx = new DataContext(_dbFolder);

        return await ctx.Players.OrderByDescending(p => p.TotalScore).ToListAsync();
    }

    public async Task Remove(int playerId)
    {
        using var ctx = new DataContext(_dbFolder);

        var foundPlayer = await Find(playerId, ctx);
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

    public async Task Update(DbPlayer dbPlayer)
    {
        using var ctx = new DataContext(_dbFolder);

        ctx.Players.Update(dbPlayer);
        await ctx.SaveChangesAsync();
    }

    public async Task<int> CountPlayers()
    {
        using var ctx = new DataContext(_dbFolder);

        return await ctx.Players.CountAsync();
    }

    private async Task<DbPlayer?> Find(int id, DataContext ctx)
    {
        return await ctx.Players.FirstOrDefaultAsync(p => p.Id == id);
    }
}
