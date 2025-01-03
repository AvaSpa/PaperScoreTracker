namespace PaperScoreTracker.Controls;

public partial class PaperCheckBox : ContentView
{
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(PaperCheckBox), false, BindingMode.TwoWay);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(PaperCheckBox), string.Empty, BindingMode.TwoWay);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public PaperCheckBox()
    {
        InitializeComponent();
    }
}