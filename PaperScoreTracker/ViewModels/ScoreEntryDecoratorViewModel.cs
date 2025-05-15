using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;

namespace PaperScoreTracker.ViewModels;

public partial class ScoreEntryDecoratorViewModel : ObservableObject
{
    private const int IncrementValue = 1;

    private readonly GameControler _gameControler;
    private readonly ScoreEntry _model;

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

    public ScoreEntryDecoratorViewModel(GameControler gameControler, ScoreEntry model)
    {
        _gameControler = gameControler;
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
        await _gameControler.UpdateScoreEntry(_model);
        //TODO: notify parent VM about the score total change (update total in score page and reorder)
    }
}
