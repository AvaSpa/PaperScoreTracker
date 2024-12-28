using Core;
using Core.Models;

namespace DataAccess.DbModels;

public class DbGameSetting
{
    public int Id { get; set; }

    public SettingName Name { get; set; }

    public string Value { get; set; }

    public DbGameSetting()
    {
    }

    public DbGameSetting(GameSetting setting) : this()
    {
        Name = setting.Name;
        Value = setting.Value;
    }

    public GameSetting ToModel() => new(Name, Value);
}
