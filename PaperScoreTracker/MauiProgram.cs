using Android.Content.Res;
using Application.Services;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using DataAccess.Repositories;
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

            builder.Services.AddSingleton<IPopupService, PopupService>();

            builder.Services.AddSingleton<MainPage, MainViewModel>();
            builder.Services.AddSingletonWithShellRoute<PlayPage, PlayViewModel>(Routes.PlayPageRoute);
            builder.Services.AddSingletonWithShellRoute<ScorePage, ScoreViewModel>(Routes.ScorePageRoute);
            builder.Services.AddTransientPopup<AddScoreEntryPopup, AddScoreEntryPopupViewModel>();

            builder.Services.AddSingleton<GameControler>();

            var playerRepository = new PlayerRepository(FileSystem.CacheDirectory);
            builder.Services.AddSingleton(playerRepository);
            var gameSettingRepository = new GameSettingRepository(FileSystem.CacheDirectory);
            builder.Services.AddSingleton(gameSettingRepository);

            //builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

            return builder.Build();
        }
    }
}
