using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;

namespace PaperScoreTracker.ViewModels;

public partial class ScoreEntryDecoratorViewModel : ObservableObject
{
    private const int IncrementValue = 1;

    private readonly GameControler _gameControler;
    private readonly PlayerDecoratorViewModel _playerVM;
    private readonly ScoreEntry _model;

    public int ScoreEntryId => _model.DbId;

    public int ScoreValue
    {
        get => _model.Value;
        set
        {
            var changed = SetProperty(_model.Value, value, _model, (model, value) => model.Value = value);

            if (changed)
                OnPropertyChanged(nameof(ScoreValue));
        }
    }

    public ScoreEntryDecoratorViewModel(GameControler gameControler, PlayerDecoratorViewModel playerVM, ScoreEntry model)
    {
        _gameControler = gameControler;
        _playerVM = playerVM;
        _model = model;
    }

    [RelayCommand]
    private void Decrement()
    {
        ScoreValue -= IncrementValue;
    }

    [RelayCommand]
    private void Increment()
    {
        ScoreValue += IncrementValue;
    }

    [RelayCommand]
    private async Task UpdateScoreEntry()
    {
        await Task.Delay(50);

        await _gameControler.UpdateScoreEntry(_model);
        _playerVM.UpdateScoreInfo(_model);
    }

    [RelayCommand]
    private async Task DeleteScoreEntry(int scoreEntryId)
    {
        await _gameControler.DeleteScoreEntry(scoreEntryId);
        _playerVM.UpdateScoreInfo(_model, true);
    }
}
