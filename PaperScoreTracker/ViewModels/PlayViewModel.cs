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


    //TODO: move in another view that has a list with alias, total score, new score entry, score add button, entry count
    //also maybe make that view the play view and the current view a new view and add here a button to navigate to it (and don't reorder at each new entry, just return from controler the ordered list
    private void AddScoreEntry(string playerAlias, int score)
    {
        var decorator = Players.FirstOrDefault(p => p.PlayerAlias == playerAlias);
        if (decorator == null)
            return;

        decorator.ScoreEntries.Add(score);

        _gameControler.AddScore(playerAlias, score);

        decorator.PlayerScore = score;

        Players = new ObservableCollection<PlayerDecoratorViewModel>(Players.OrderByDescending(p => p.PlayerScore));
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
                AddScoreEntry(playerDecorator.PlayerAlias, newScoreEntry);
            }
        }
        //
    }
}
