using PaperScoreTracker.ViewModels;

namespace PaperScoreTracker.Views;

public partial class PlayPage : ContentPage
{
    public PlayPage(PlayViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}