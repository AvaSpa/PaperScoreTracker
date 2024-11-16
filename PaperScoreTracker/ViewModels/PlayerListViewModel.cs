using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerListViewModel : ObservableObject
{
    protected readonly GameControler _gameControler;

    [ObservableProperty]
    private string _gameName;

    [ObservableProperty]
    private ObservableCollection<PlayerDecoratorViewModel> _players;

    public PlayerListViewModel(GameControler gameControler)
    {
        _gameControler = gameControler;
        _players = new ObservableCollection<PlayerDecoratorViewModel>();
    }

    [RelayCommand]
    protected async Task Appearing()
    {
        await LoadGameData();
    }

    protected async Task LoadGameData()
    {
        GameName = _gameControler.GameName;
        Players.Clear();

        var players = await _gameControler.GetAllPlayers();
        foreach (var player in players)
        {
            Players.Add(new PlayerDecoratorViewModel(_gameControler, player));
        }
    }
}
