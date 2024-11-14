using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Platforms.Android;
using PaperScoreTracker.Services;

namespace PaperScoreTracker.ViewModels;

public partial class PlayViewModel : PlayerListViewModel
{
    public PlayViewModel(GameControler playerControler) : base(playerControler)
    {
    }

    [RelayCommand]
    private async Task ViewScores()
    {
        KeyboardHelper.HideKeyboard();

        await Shell.Current.GoToAsync(Routes.ScorePageRoute);
    }
}
