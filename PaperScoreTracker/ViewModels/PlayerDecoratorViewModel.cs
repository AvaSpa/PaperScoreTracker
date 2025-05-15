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

    [ObservableProperty]
    private ScoreEntryDecoratorViewModel _latestScoreEntry;

    [ObservableProperty]
    private IEnumerable<ScoreEntryDecoratorViewModel> _scoreEntries;

    public PlayerDecoratorViewModel(GameControler gameControler, Player model)
    {
        _gameControler = gameControler;

        Model = model;
        UpdateScoreEntries();

        LatestScoreEntry = new ScoreEntryDecoratorViewModel(_gameControler, new ScoreEntry(model, 0));
    }

    [RelayCommand]
    private async Task SaveLatestScoreEntry()
    {
        var updatedPlayer = await _gameControler.AddPlayerScore(PlayerAlias, LatestScoreEntry.ScoreValue);
        if (updatedPlayer == null)
            return;

        Model.ScoreEntries.Clear();
        Model.ScoreEntries.AddRange(updatedPlayer.ScoreEntries);
        Model.TotalScore = updatedPlayer.TotalScore;

        LatestScoreEntry.ScoreValue = 0;
        UpdateScoreEntries();

        OnPropertyChanged(nameof(PlayerScore));
    }

    [RelayCommand]
    private async Task UpdatePlayerAlias()
    {
        await _gameControler.UpdateAlias(Model);
    }

    private void UpdateScoreEntries()
    {
        ScoreEntries = Model.ScoreEntries.Select(e => new ScoreEntryDecoratorViewModel(_gameControler, e)).ToList();
    }
}
