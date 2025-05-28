using Application.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using PaperScoreTracker.Platforms.Android;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerDecoratorViewModel : ObservableObject
{
    private readonly GameControler _gameControler;
    private readonly IPopupService _popupService;
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

    public PlayerDecoratorViewModel(GameControler gameControler, IPopupService popupService, Player model, PlayerListViewModel parent)
    {
        _gameControler = gameControler;
        _popupService = popupService;
        _parent = parent;

        Model = model;
        UpdateScoreEntries();

        LatestScoreEntry = new ScoreEntryDecoratorViewModel(_gameControler, this, new ScoreEntry(model, 0));
    }

    /// <summary>
    /// NOTE: This is used in the PlayPage. Keeping it here in case the PlayPage comes back.
    /// Will also keep LatestScoreEntry and use it for the ScorePage add as well.
    /// </summary>
    /// <returns></returns>
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

    [RelayCommand]
    private async Task AddScoreEntry()
    {
        var popupResult = await _popupService.ShowPopupAsync<AddScoreEntryPopupViewModel>();

        KeyboardHelper.HideKeyboard();

        if (popupResult == null)
            return;

        if (popupResult is not int scoreValue)
            return;

        LatestScoreEntry.ScoreValue = scoreValue;
        await SaveLatestScoreEntry();
    }

    internal void UpdateScoreInfo(ScoreEntry updatedScoreEntry, bool isDeleted = false)
    {
        if (isDeleted)
            Model.ScoreEntries.RemoveAll(se => se.DbId == updatedScoreEntry.DbId);
        else
        {
            var currentScoreEntry = Model.ScoreEntries.FirstOrDefault(se => se.DbId == updatedScoreEntry.DbId);
            if (currentScoreEntry != null)
                currentScoreEntry.Value = updatedScoreEntry.Value;
        }

        Model.TotalScore = Model.ScoreEntries.Sum(se => se.Value);
        OnPropertyChanged(nameof(PlayerScore));
    }

    private void UpdateScoreEntries()
    {
        ScoreEntries = [.. Model.ScoreEntries.Select(e => new ScoreEntryDecoratorViewModel(_gameControler, this, e))];
    }
}
