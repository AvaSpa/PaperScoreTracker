using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace PaperScoreTracker;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly DataContext _dataContext;

    public App(DataContext dataContext)
    {
        InitializeComponent();

        MainPage = new AppShell();

        _dataContext = dataContext;
    }

    protected override void OnStart()
    {
        _dataContext.Database.Migrate();
    }
}
