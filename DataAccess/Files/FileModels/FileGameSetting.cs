using Core;
using Core.Models;

namespace DataAccess.Files.FileModels;

public class FileGameSetting
{
    public int Id { get; set; }

    public SettingName Name { get; set; }

    public string Value { get; set; }

    public FileGameSetting()
    {
    }

    public FileGameSetting(GameSetting setting) : this()
    {
        Name = setting.Name;
        Value = setting.Value;
    }

    public GameSetting ToModel() => new(Name, Value);
}
