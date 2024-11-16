using DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class PlayerRepository
{
    public PlayerRepository()
    {
    }

    public async Task<IEnumerable<DbPlayer>> GetAllPlayers()
    {
        using var ctx = new DataContext();

        return await ctx.Players.OrderByDescending(p => p.TotalScore).ToListAsync();
    }
}
