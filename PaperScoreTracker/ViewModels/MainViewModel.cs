using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Exceptions;
using Core.Models;
using PaperScoreTracker.Platforms.Android;
using PaperScoreTracker.Utils;

namespace PaperScoreTracker.ViewModels;

public partial class MainViewModel : PlayerListViewModel
{
    [ObservableProperty]
    private string _playerAlias;

    [ObservableProperty]
    private bool _reverseScoring;

    public MainViewModel(GameControler playerControler) : base(playerControler)
    {
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

            Players.Add(new PlayerDecoratorViewModel(_gameControler, newPlayer));

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
        var playerCount = await _gameControler.GetPlayerCount();

        if (playerCount == 0)
        {
            await NotificationSingleton.Instance.ShowToast("No players!");

            return;
        }

        if (!string.IsNullOrWhiteSpace(GameName))
            _gameControler.SetGameName(GameName);
        _gameControler.SetReverseScoring(ReverseScoring);

        KeyboardHelper.HideKeyboard();

        await Shell.Current.GoToAsync(Routes.PlayPageRoute);
    }

    [RelayCommand]
    private async Task ResetGame()
    {
        GameName = string.Empty;
        ReverseScoring = false;
        Players.Clear();

        _gameControler.SetGameName(GameName);
        _gameControler.SetReverseScoring(ReverseScoring);
        await _gameControler.ClearPlayers();
    }

    [RelayCommand]
    private async Task ResetScores()
    {
        await _gameControler.ClearScores();

        await NotificationSingleton.Instance.ShowToast("All scores were reset.");
    }
}
