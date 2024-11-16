using Core.Models;
using DataAccess.DbModels;

namespace Application;

internal static class MappingHelper
{
    public static IEnumerable<Player> MapDbPlayerList(IEnumerable<DbPlayer> dbPlayers)
    {
        var result = new List<Player>();

        foreach (var dbPlayer in dbPlayers)
            result.Add(dbPlayer.ToModel());

        return result;
    }
}
