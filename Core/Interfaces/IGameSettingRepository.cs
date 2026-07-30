using Core.Models;

namespace Core.Interfaces;

public interface IGameSettingRepository
{
    Task<string> GetGameSettingValue(SettingName settingName);
    Task SaveGameSetting(GameSetting setting);
}
