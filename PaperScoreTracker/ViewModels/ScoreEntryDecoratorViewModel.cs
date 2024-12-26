using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;

namespace PaperScoreTracker.ViewModels;

public partial class ScoreEntryDecoratorViewModel : ObservableObject
{
    private ScoreEntry _model;

    [ObservableProperty]
    private int _incrementValue = 1;

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

    public ScoreEntryDecoratorViewModel(ScoreEntry model)
    {
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
}
