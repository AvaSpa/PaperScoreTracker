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

    public async Task<IEnumerable<DbPlayer>> GetAllPlayers()
    {
        using var ctx = new DataContext(_dbFolder);

        return await ctx.Players.OrderByDescending(p => p.TotalScore).ToListAsync();
    }
}
