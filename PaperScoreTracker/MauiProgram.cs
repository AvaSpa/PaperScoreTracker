using Application.Services;
using CommunityToolkit.Maui;
using DataAccess;
using Microsoft.Extensions.Logging;
using PaperScoreTracker.ViewModels;
using PaperScoreTracker.Views;

namespace PaperScoreTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage, MainViewModel>();
            builder.Services.AddSingletonWithShellRoute<PlayPage, PlayViewModel>(Routes.PlayPageRoute);
            builder.Services.AddSingletonWithShellRoute<ScorePage, ScoreViewModel>(Routes.ScorePageRoute);

            builder.Services.AddSingleton<GameControler>();

            var playerRepository = new PlayerRepository(FileSystem.AppDataDirectory);
            builder.Services.AddSingleton(playerRepository);

            //builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
