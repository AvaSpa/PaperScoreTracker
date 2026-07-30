using Core.Models;
using DataAccess.SQLiteDb.DbModels;

namespace Application;

internal static class DBMappingHelper
{
    public static IEnumerable<Player> MapDbPlayerList(IEnumerable<DbPlayer> dbPlayers)
    {
        var result = new List<Player>();

        foreach (var dbPlayer in dbPlayers)
            result.Add(dbPlayer.ToModel());

        return result;
    }
}