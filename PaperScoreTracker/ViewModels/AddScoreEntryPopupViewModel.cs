using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PaperScoreTracker.ViewModels;

public partial class AddScoreEntryPopupViewModel : ObservableObject
{
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private int _score;

    public AddScoreEntryPopupViewModel(IPopupService popupService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    private void Submit()
    {
        _popupService.ClosePopup(Score);
    }
}
