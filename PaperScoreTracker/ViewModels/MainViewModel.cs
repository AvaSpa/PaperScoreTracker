using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Models;
using PaperScoreTracker.Services;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly GameControler _gameControler;

    [ObservableProperty]
    private string _gameName;

    [ObservableProperty]
    private string _playerAlias;

    [ObservableProperty]
    private ObservableCollection<PlayerDecoratorViewModel> _players;

    public MainViewModel(GameControler playerControler)
    {
        _gameControler = playerControler;

        Players = new ObservableCollection<PlayerDecoratorViewModel>();
    }

    [RelayCommand]
    private void Add()
    {
        if (string.IsNullOrWhiteSpace(PlayerAlias))
            return;

        var newPlayer = new Player(PlayerAlias);

        _gameControler.AddPlayer(newPlayer);

        Players.Add(new PlayerDecoratorViewModel(newPlayer));

        PlayerAlias = string.Empty;
    }

    [RelayCommand]
    private void Delete(string playerAlias)
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

        await Shell.Current.GoToAsync("play");
    }

    [RelayCommand]
    private void ResetGame()
    {
        _gameControler.SetGameName(GameName);
        _gameControler.ClearPlayers();

        Players.Clear();
    }


    [RelayCommand]
    private async Task Appearing()
    {
        await LoadPlayers();
    }

    private async Task LoadPlayers()
    {
        var players = await _gameControler.GetAllPlayers();
        foreach (var player in players)
        {
            Players.Add(new PlayerDecoratorViewModel(player));
        }
    }
}
