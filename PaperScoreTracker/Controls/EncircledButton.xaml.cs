using System.Windows.Input;

namespace PaperScoreTracker.Controls;

public partial class EncircledButton : ContentView
{
    public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(EncircledButton), string.Empty);
    public static readonly BindableProperty ClickCommandProperty = BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(EncircledButton), null);
    public static readonly BindableProperty ButtonTextFontSizeProperty = BindableProperty.Create(nameof(ButtonTextFontSize), typeof(double), typeof(EncircledButton), 20d);

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public ICommand ClickCommand
    {
        get => (ICommand)GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public double ButtonTextFontSize
    {
        get => (double)GetValue(ButtonTextFontSizeProperty);
        set => SetValue(ButtonTextFontSizeProperty, value);
    }

    public EncircledButton()
    {
        InitializeComponent();
    }
}