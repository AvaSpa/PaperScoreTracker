using CommunityToolkit.Mvvm.ComponentModel;

namespace PaperScoreTracker.ViewModels;

public partial class PlayViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title;

}
