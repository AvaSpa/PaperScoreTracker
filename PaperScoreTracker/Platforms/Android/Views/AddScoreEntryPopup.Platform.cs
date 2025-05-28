using Android.Widget;
using Microsoft.Maui.Handlers;

namespace PaperScoreTracker.Views;

public partial class AddScoreEntryPopup
{
    private partial void SetCaretToEnd(Entry entry)
    {
        var handler = entry.Handler as EntryHandler;
        if (handler?.PlatformView is EditText editText)
        {
            editText.SetSelection(editText.Text?.Length ?? 0);
        }
    }
}
