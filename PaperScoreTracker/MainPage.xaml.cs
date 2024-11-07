using PaperScoreTracker.ViewModels;

namespace PaperScoreTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}
