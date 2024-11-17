using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using PaperScoreTracker.Platforms.Android;
using PaperScoreTracker.Utils;

namespace PaperScoreTracker.ViewModels;

public partial class MainViewModel : PlayerListViewModel
{
    [ObservableProperty]
    private string _playerAlias;

    public MainViewModel(GameControler playerControler) : base(playerControler)
    {
    }

    [RelayCommand]
    private async Task AddPlayer()
    {
        if (string.IsNullOrWhiteSpace(PlayerAlias))
            return;

        var newPlayer = new Player(PlayerAlias);

        await _gameControler.AddPlayer(newPlayer);

        Players.Add(new PlayerDecoratorViewModel(_gameControler, newPlayer));

        PlayerAlias = string.Empty;
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
    private async Task Start()
    {
        var playerCount = await _gameControler.GetPlayerCount();

        if (playerCount == 0)
        {
            await NotificationSingleton.Instance.ShowToast("No players!");

            return;
        }

        if (!string.IsNullOrWhiteSpace(GameName))
            _gameControler.SetGameName(GameName);

        KeyboardHelper.HideKeyboard();

        await Shell.Current.GoToAsync(Routes.PlayPageRoute);
    }

    [RelayCommand]
    private void ResetGame()
    {
        if (string.IsNullOrWhiteSpace(GameName))
            return;

        _gameControler.SetGameName(GameName);
        _gameControler.ClearPlayers();

        Players.Clear();
    }
}
