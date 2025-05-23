using Application.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Platforms.Android;

namespace PaperScoreTracker.ViewModels;

public partial class PlayViewModel : PlayerListViewModel
{
    public PlayViewModel(GameControler playerControler, IPopupService popupService) : base(playerControler, popupService)
    {
    }

    [RelayCommand]
    private async Task ViewScores()
    {
        KeyboardHelper.HideKeyboard();

        await Task.Delay(50);

        await Shell.Current.GoToAsync(Routes.ScorePageRoute);
    }
}
