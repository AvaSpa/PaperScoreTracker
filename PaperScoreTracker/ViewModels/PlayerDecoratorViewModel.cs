using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using System.Threading.Tasks;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerDecoratorViewModel : ObservableObject
{
    private readonly GameControler _gameControler;
    private readonly PlayerListViewModel _parent;

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

    public PlayerDecoratorViewModel(GameControler gameControler, Player model, PlayerListViewModel parent)
    {
        _gameControler = gameControler;
        _parent = parent;

        Model = model;
        UpdateScoreEntries();

        LatestScoreEntry = new ScoreEntryDecoratorViewModel(_gameControler, this, new ScoreEntry(model, 0));
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

    internal async Task UpdateScoreInfo(ScoreEntry updatedScoreEntry)
    {
        var currentScoreEntry = Model.ScoreEntries.FirstOrDefault(se => se.DbId == updatedScoreEntry.DbId);
        if (currentScoreEntry != null)
            currentScoreEntry.Value = updatedScoreEntry.Value;

        Model.TotalScore = Model.ScoreEntries.Sum(se => se.Value);
        OnPropertyChanged(nameof(PlayerScore));

      await  _parent.ReloadPlayers();
    }

    private void UpdateScoreEntries()
    {
        ScoreEntries = [.. Model.ScoreEntries.Select(e => new ScoreEntryDecoratorViewModel(_gameControler, this, e))];
    }
}
