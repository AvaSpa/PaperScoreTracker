using Application.Services;
using CommunityToolkit.Maui.Core;

namespace PaperScoreTracker.ViewModels;

public partial class ScoreViewModel : PlayerListViewModel
{
    public ScoreViewModel(GameControler gameControler, IPopupService popupService) : base(gameControler, popupService, true)
    {
    }
}
