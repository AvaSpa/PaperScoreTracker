using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerDecoratorViewModel : ObservableObject
{
    private readonly GameControler _gameControler;

    public Player Model { get; }

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

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveLatestScoreEntryCommand))]
    private int? _latestScoreEntry;

    public PlayerDecoratorViewModel(GameControler gameControler, Player model)
    {
        _gameControler = gameControler;

        Model = model;
    }

    private bool CanSaveLatestScoreEntry() => LatestScoreEntry.HasValue;

    [RelayCommand(CanExecute = nameof(CanSaveLatestScoreEntry))]
    private async Task SaveLatestScoreEntry()
    {
        if (!LatestScoreEntry.HasValue)
            return;

        var updatedPlayer = await _gameControler.AddPlayerScore(PlayerAlias, LatestScoreEntry.Value);
        if (updatedPlayer == null)
            return;

        Model.ScoreEntries.Clear();
        Model.ScoreEntries.AddRange(updatedPlayer.ScoreEntries);
        Model.TotalScore = updatedPlayer.TotalScore;

        LatestScoreEntry = null;

        OnPropertyChanged(nameof(PlayerScore));
        OnPropertyChanged(nameof(ScoreEntries));
    }
}
