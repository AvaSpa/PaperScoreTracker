namespace PaperScoreTracker;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Light;//TODO: for the future add another white ellipsis for encircled button and change the popup background (and all other hardcoded colors)
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
