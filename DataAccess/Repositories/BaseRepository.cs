namespace DataAccess.Repositories;

public class BaseRepository
{
    protected readonly string _dbFolder;

    public BaseRepository(string dbFolder)
    {
        _dbFolder = dbFolder;

        using var ctx = new DataContext(_dbFolder);
        ctx.Initialize();
    }
}
