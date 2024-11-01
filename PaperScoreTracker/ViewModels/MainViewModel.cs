using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PaperScoreTracker.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        private int _counter;

        [ObservableProperty]
        private string _buttonText;

        public MainViewModel()
        {
            _buttonText = "Counter";
        }

        [RelayCommand]
        private void Count()
        {
            _counter++;
            ButtonText = $"Counter: {_counter}";
        }
    }
}
