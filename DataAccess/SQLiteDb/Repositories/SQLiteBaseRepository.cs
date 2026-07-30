namespace DataAccess.SQLiteDb.Repositories;

public class SQLiteBaseRepository
{
    protected readonly string _dbFolder;

    public SQLiteBaseRepository(string dbFolder)
    {
        _dbFolder = dbFolder;

        using var ctx = new DataContext(_dbFolder);
        ctx.Initialize();
    }
}
