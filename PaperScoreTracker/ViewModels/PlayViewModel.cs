using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Services;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class PlayViewModel : ObservableObject
{
    private readonly GameControler _playerControler;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private ObservableCollection<PlayerDecoratorViewModel> _players;

    public PlayViewModel(GameControler playerControler)
    {
        _playerControler = playerControler;
        _players = new ObservableCollection<PlayerDecoratorViewModel>();
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await LoadPlayers();
    }

    private async Task LoadPlayers()
    {
        var players = await _playerControler.GetAllPlayers();
        foreach (var player in players)
        {
            Players.Add(new PlayerDecoratorViewModel(player));
        }
    }
}
