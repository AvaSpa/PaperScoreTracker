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

    private async void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        NewScoreEntry.Focus();

        await Task.Delay(100);

        SetCaretToEnd(NewScoreEntry);
    }

    private partial void SetCaretToEnd(Entry entry);
}