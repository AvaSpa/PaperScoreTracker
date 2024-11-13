using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Services;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class PlayViewModel : ObservableObject
{
    private readonly GameControler _gameControler;

    [ObservableProperty]
    private string _gameName;

    [ObservableProperty]
    private ObservableCollection<PlayerDecoratorViewModel> _players;

    public PlayViewModel(GameControler playerControler)
    {
        _gameControler = playerControler;
        _players = new ObservableCollection<PlayerDecoratorViewModel>();
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await LoadPlayers();
    }

    private async Task LoadPlayers()
    {
        GameName = _gameControler.GameName;

        Players.Clear();

        var players = await _gameControler.GetAllPlayers();
        foreach (var player in players)
        {
            Players.Add(new PlayerDecoratorViewModel(_gameControler, player));
        }

        //TEST
        var r = new Random();

        foreach (var playerDecorator in Players)
        {
            for (var i = 0; i < 10; i++)
            {
                var newScoreEntry = r.Next(-20, 20);
                playerDecorator.AddScoreEntryCommand.Execute(newScoreEntry);
            }
        }
        //
    }
}
