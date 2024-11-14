using CommunityToolkit.Maui.Alerts;

namespace PaperScoreTracker.Services;

public class NotificationSingleton
{
    public static NotificationSingleton Instance { get; private set; }

    static NotificationSingleton()
    {
        Instance ??= new NotificationSingleton();
    }

    public async Task ShowToast(string message)
    {
        var toast = Toast.Make(message);

        await toast.Show();
        await Task.Delay(2500);
    }
}
