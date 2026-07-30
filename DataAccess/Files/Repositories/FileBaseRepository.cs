using System.Text.Json;

namespace DataAccess.Files.Repositories;

public class FileBaseRepository
{
    protected readonly string _filesFolder;
    protected readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public FileBaseRepository(string filesFolder)
    {
        _filesFolder = filesFolder;

        if (!Directory.Exists(_filesFolder))
            Directory.CreateDirectory(_filesFolder);

        EnsureFile("players.json");
        EnsureFile("gamesettings.json");
    }

    protected string GetPath(string fileName) => Path.Combine(_filesFolder, fileName);

    private void EnsureFile(string fileName)
    {
        var path = GetPath(fileName);
        if (!File.Exists(path))
            File.WriteAllText(path, "[]");
    }
}
