using Application.Services;

namespace PaperScoreTracker.ViewModels;

public partial class ScoreViewModel : PlayerListViewModel
{
    public ScoreViewModel(GameControler gameControler) : base(gameControler)
    {
    }
}
