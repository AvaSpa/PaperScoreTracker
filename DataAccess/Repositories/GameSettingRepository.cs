using Core;
using Core.Models;
using DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class GameSettingRepository : BaseRepository
{
    public GameSettingRepository(string dbFolder) : base(dbFolder)
    {
    }

    public async Task<string> GetGameSettingValue(SettingName settingName)
    {
        using var ctx = new DataContext(_dbFolder);

        var foundSetting = await ctx.GameSettings.FirstOrDefaultAsync(s => s.Name == settingName);

        return foundSetting?.Value ?? string.Empty;
    }

    public async Task SaveGameSetting(GameSetting setting)
    {
        using var ctx = new DataContext(_dbFolder);

        var foundSetting = await ctx.GameSettings.FirstOrDefaultAsync(s => s.Name == setting.Name);

        if (foundSetting != null)
        {
            foundSetting.Value = setting.Value;

            ctx.Update(foundSetting);
        }
        else
        {
            var dbSetting = new DbGameSetting(setting);

            ctx.Add(dbSetting);
        }

        await ctx.SaveChangesAsync();
    }
}
