namespace Core.Models;

public class GameSetting
{
    public SettingName Name { get; set; }

    public string Value { get; set; }

    public GameSetting(SettingName name, string value)
    {
        Name = name;

        Value = value;
    }
}
