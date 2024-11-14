using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Models;
using PaperScoreTracker.Services;

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

    public IList<int> ScoreEntries => Model.ScoreEntries;

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
    private void SaveLatestScoreEntry()
    {
        if (!LatestScoreEntry.HasValue)
            return;

        var updatedPlayer = _gameControler.AddPlayerScore(PlayerAlias, LatestScoreEntry.Value);
        LatestScoreEntry = null;

        if (updatedPlayer != null)
        {
            OnPropertyChanged(nameof(PlayerScore));
            OnPropertyChanged(nameof(ScoreEntries));
        }
    }
}
