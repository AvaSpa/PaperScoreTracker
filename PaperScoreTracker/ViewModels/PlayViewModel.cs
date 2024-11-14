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

    //TODO: replace player-score grid with alias, total score, new score entry, score add button, entry count; navigate to score button
    //make new view for the player-score grid
    private async Task AddScoreEntry(string playerAlias, int score)
    {
        var decorator = Players.FirstOrDefault(p => p.PlayerAlias == playerAlias);
        if (decorator == null)
            return;

        decorator.ScoreEntries.Add(score);

        _gameControler.AddScore(playerAlias, score);

        decorator.PlayerScore = score;
    }
}
