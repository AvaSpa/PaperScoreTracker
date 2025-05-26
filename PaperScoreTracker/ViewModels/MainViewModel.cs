using Application.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Exceptions;
using Core.Models;
using PaperScoreTracker.Platforms.Android;
using PaperScoreTracker.Utils;

namespace PaperScoreTracker.ViewModels;

public partial class MainViewModel : PlayerListViewModel
{
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private string _playerAlias;

    public MainViewModel(GameControler playerControler, IPopupService popupService) : base(playerControler, popupService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task AddPlayer()
    {
        if (string.IsNullOrWhiteSpace(PlayerAlias))
            return;

        var newPlayer = new Player(PlayerAlias);

        try
        {
            await _gameControler.AddPlayer(newPlayer);

            Players.Add(new PlayerDecoratorViewModel(_gameControler, _popupService, newPlayer, this));

            PlayerAlias = string.Empty;
        }
        catch (AddPlayerException e)
        {
            await NotificationSingleton.Instance.ShowToast(e.Message);
        }
    }

    [RelayCommand]
    private async Task DeletePlayer(string playerAlias)
    {
        await _gameControler.RemovePlayer(playerAlias);

        var foundPlayer = Players.FirstOrDefault(p => p.PlayerAlias == playerAlias);
        if (foundPlayer != null)
            Players.Remove(foundPlayer);
    }

    [RelayCommand]
    private async Task StartGame()
    {
        if (!string.IsNullOrWhiteSpace(GameName))
            await _gameControler.SetGameName(GameName);
        await _gameControler.SetReverseScoring(ReverseScoring);

        KeyboardHelper.HideKeyboard();

        await Task.Delay(50);

        await Shell.Current.GoToAsync(Routes.ScorePageRoute);
    }

    [RelayCommand]
    private async Task ResetGame()
    {
        GameName = string.Empty;
        ReverseScoring = false;
        Players.Clear();

        await _gameControler.SetGameName(GameName);
        await _gameControler.SetReverseScoring(ReverseScoring);
        await _gameControler.ClearPlayers();
    }

    [RelayCommand]
    private async Task ResetScores()
    {
        await _gameControler.ClearScores();

        await NotificationSingleton.Instance.ShowToast("All scores were reset.");
    }

    [RelayCommand]
    private async Task LaunchPrivacy()
    {
        await Browser.Default.OpenAsync(
           new Uri("https://avaspa.azurewebsites.net/appprivacypolicy.html"),
           new BrowserLaunchOptions
           {
               LaunchMode = BrowserLaunchMode.SystemPreferred,
               TitleMode = BrowserTitleMode.Show
           });
    }
}
