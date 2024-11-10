using CommunityToolkit.Mvvm.ComponentModel;
using PaperScoreTracker.Models;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerDecoratorViewModel : ObservableObject
{
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

    public PlayerDecoratorViewModel(Player model)
    {
        Model = model;

        _scoreEntries = new ObservableCollection<int>();
    }

    public void AddScoreEntry(int score)
    {
        ScoreEntries.Add(score);

        PlayerScore = ScoreEntries.Sum();
    }
}
