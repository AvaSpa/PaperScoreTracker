using CommunityToolkit.Maui.Views;
using PaperScoreTracker.ViewModels;

namespace PaperScoreTracker.Views;

public partial class AddScoreEntryPopup : Popup
{
    public AddScoreEntryPopup(AddScoreEntryPopupViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}