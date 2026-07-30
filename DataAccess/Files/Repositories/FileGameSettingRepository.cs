using Core;
using Core.Models;
using DataAccess.Files.FileModels;
using System.Text.Json;

namespace DataAccess.Files.Repositories;

public class FileGameSettingRepository : FileBaseRepository
{
    private const string FileName = "gamesettings.json";

    public FileGameSettingRepository(string filesFolder) : base(filesFolder)
    {
    }

    public async Task<string> GetGameSettingValue(SettingName settingName)
    {
        var path = GetPath(FileName);
        var text = await File.ReadAllTextAsync(path);
        var items = JsonSerializer.Deserialize<List<FileGameSetting>>(text, _serializerOptions) ?? new List<FileGameSetting>();

        var found = items.FirstOrDefault(s => s.Name == settingName);
        return found?.Value ?? string.Empty;
    }

    public async Task SaveGameSetting(GameSetting setting)
    {
        var path = GetPath(FileName);
        var text = await File.ReadAllTextAsync(path);
        var items = JsonSerializer.Deserialize<List<FileGameSetting>>(text, _serializerOptions) ?? new List<FileGameSetting>();

        var found = items.FirstOrDefault(s => s.Name == setting.Name);
        if (found != null)
        {
            found.Value = setting.Value;
        }
        else
        {
            var id = items.Any() ? items.Max(i => i.Id) + 1 : 1;
            var newItem = new FileGameSetting(setting) { Id = id };
            items.Add(newItem);
        }

        var outText = JsonSerializer.Serialize(items, _serializerOptions);
        await File.WriteAllTextAsync(path, outText);
    }
}
