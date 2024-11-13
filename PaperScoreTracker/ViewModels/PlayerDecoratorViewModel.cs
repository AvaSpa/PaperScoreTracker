using CommunityToolkit.Mvvm.ComponentModel;
using PaperScoreTracker.Models;
using PaperScoreTracker.Services;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerDecoratorViewModel : ObservableObject
{
    private readonly GameControler _gameControler;

    public Player Model { get; }

    public string PlayerAlias
    {
        get => Model.Alias;
        set => SetProperty(Model.Alias, value, Model, (model, alias) => model.Alias = alias);
    }

    public int PlayerScore
    {
        get => Model.Score;
        set => SetProperty(Model.Score, value, Model, (model, score) => model.Score = score);
    }

    [ObservableProperty]
    private ObservableCollection<int> _scoreEntries;

    public PlayerDecoratorViewModel(GameControler gameControler, Player model)
    {
        _gameControler = gameControler;

        Model = model;

        _scoreEntries = new ObservableCollection<int>();
    }
}
