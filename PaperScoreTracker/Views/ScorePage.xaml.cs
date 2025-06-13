using PaperScoreTracker.ViewModels;

namespace PaperScoreTracker.Views;

public partial class ScorePage : ContentPage
{
    public ScorePage(ScoreViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DeviceDisplay.Current.KeepScreenOn = true;
    }

    protected override void OnDisappearing()
    {
        DeviceDisplay.Current.KeepScreenOn = false;
        base.OnDisappearing();
    }
}