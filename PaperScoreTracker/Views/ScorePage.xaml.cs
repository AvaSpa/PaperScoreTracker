using PaperScoreTracker.ViewModels;

namespace PaperScoreTracker.Views;

public partial class ScorePage : ContentPage
{
    public ScorePage(ScoreViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}