using Application.Services;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Core.Interfaces;
using DataAccess.SQLiteDb.Repositories;
using Microsoft.Extensions.Logging;
using PaperScoreTracker.ViewModels;
using PaperScoreTracker.Views;
using Plugin.AdMob;

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
                .UseAdMob()
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

            var sqlitePlayerRepo = new SQLitePlayerRepository(FileSystem.CacheDirectory);
            builder.Services.AddSingleton(sqlitePlayerRepo);
            builder.Services.AddSingleton<IPlayerRepository>(sp => sp.GetRequiredService<SQLitePlayerRepository>());

            var sqliteGameSettingRepo = new SQLiteGameSettingRepository(FileSystem.CacheDirectory);
            builder.Services.AddSingleton(sqliteGameSettingRepo);
            builder.Services.AddSingleton<IGameSettingRepository>(sp => sp.GetRequiredService<SQLiteGameSettingRepository>());

            //builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

            return builder.Build();
        }
    }
}
