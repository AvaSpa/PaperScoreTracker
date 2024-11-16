using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Experiments
{
    public partial class App : Application
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
}
