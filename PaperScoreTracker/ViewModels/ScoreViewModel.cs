using Application.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;

namespace PaperScoreTracker.ViewModels;

public partial class ScoreViewModel : PlayerListViewModel
{
    public ScoreViewModel(GameControler gameControler, IPopupService popupService) : base(gameControler, popupService)
    {
    }

    [RelayCommand]
    private async Task RankPlayers() => await ReloadPlayers();
}
