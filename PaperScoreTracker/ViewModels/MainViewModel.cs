using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Models;
using PaperScoreTracker.Platforms.Android;
using PaperScoreTracker.Services;

namespace PaperScoreTracker.ViewModels;

public partial class MainViewModel : PlayerListViewModel
{
    [ObservableProperty]
    private string _playerAlias;

    public MainViewModel(GameControler playerControler) : base(playerControler)
    {
    }

    [RelayCommand]
    private void AddPlayer()
    {
        if (string.IsNullOrWhiteSpace(PlayerAlias))
            return;

        var newPlayer = new Player(PlayerAlias);

        _gameControler.AddPlayer(newPlayer);

        Players.Add(new PlayerDecoratorViewModel(_gameControler, newPlayer));

        PlayerAlias = string.Empty;
    }

    [RelayCommand]
    private void DeletePlayer(string playerAlias)
    {
        _gameControler.RemovePlayer(playerAlias);

        var foundPlayer = Players.FirstOrDefault(p => p.PlayerAlias == playerAlias);
        if (foundPlayer != null)
            Players.Remove(foundPlayer);
    }

    [RelayCommand]
    private async Task Start()
    {
        var players = await _gameControler.GetAllPlayers();

        if (!players.Any())
        {
            await NotificationSingleton.Instance.ShowToast("No players!");

            return;
        }

        if (!string.IsNullOrWhiteSpace(GameName))
            _gameControler.SetGameName(GameName);

        KeyboardHelper.HideKeyboard();

        await Shell.Current.GoToAsync("play");
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
