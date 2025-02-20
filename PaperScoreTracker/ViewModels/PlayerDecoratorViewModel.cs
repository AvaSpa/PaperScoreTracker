using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerDecoratorViewModel : ObservableObject
{
    private readonly GameControler _gameControler;

    [ObservableProperty]
    private Player _model;

    [ObservableProperty]
    private bool _isLabelVisible = true;

    [ObservableProperty]
    private bool _isEntryVisible;

    public ScoreEntryDecoratorViewModel ScoreEntryDecoratorViewModel { get; }

    public string PlayerAlias
    {
        get => Model.Alias;
        set
        {
            var changed = SetProperty(Model.Alias, value, Model, (model, alias) => model.Alias = alias);

            if (changed)
                OnPropertyChanged(nameof(PlayerAlias));
        }
    }

    public int PlayerScore => Model.TotalScore;

    public IEnumerable<int> ScoreEntries => Model.ScoreEntries.Select(e => e.Value).ToList();

    public PlayerDecoratorViewModel(GameControler gameControler, Player model)
    {
        _gameControler = gameControler;

        Model = model;
        ScoreEntryDecoratorViewModel = new ScoreEntryDecoratorViewModel(new ScoreEntry(0));
    }

    [RelayCommand]
    private async Task SaveLatestScoreEntry()
    {
        var updatedPlayer = await _gameControler.AddPlayerScore(PlayerAlias, ScoreEntryDecoratorViewModel.ScoreValue);
        if (updatedPlayer == null)
            return;

        Model.ScoreEntries.Clear();
        Model.ScoreEntries.AddRange(updatedPlayer.ScoreEntries);
        Model.TotalScore = updatedPlayer.TotalScore;

        ScoreEntryDecoratorViewModel.ScoreValue = 0;

        OnPropertyChanged(nameof(PlayerScore));
        OnPropertyChanged(nameof(ScoreEntries));
    }

    [RelayCommand]
    private void BeginAliasEdit()
    {
        IsLabelVisible = false;
        IsEntryVisible = true;
    }

    [RelayCommand]
    private async Task EndAliasEdit()
    {
        IsLabelVisible = true;
        IsEntryVisible = false;

        await _gameControler.UpdateAlias(Model);
    }
}
