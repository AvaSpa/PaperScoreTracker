
using CommunityToolkit.Mvvm.Input;

namespace PaperScoreTracker.Controls;

public partial class PaperCheckBox : ContentView
{
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(PaperCheckBox), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedPropertyChanged);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(PaperCheckBox), string.Empty, BindingMode.TwoWay);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set
        {
            SetValue(IsCheckedProperty, value);

            ToggleVisual(value);
        }
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public IRelayCommand ToggleStateCommand => new RelayCommand(ToggleCheckedState);

    public PaperCheckBox()
    {
        InitializeComponent();
    }

    private static void OnIsCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var newIsChecked = (bool)newValue;
        var oldIsChecked = (bool)oldValue;

        if (newIsChecked == oldIsChecked)
            return;

        if (bindable is not PaperCheckBox paperCheckBox)
            return;

        paperCheckBox.IsChecked = newIsChecked;
    }

    private void ToggleCheckedState()
    {
        IsChecked = !IsChecked;
    }

    private void ToggleVisual(bool isChecked)
    {
        CheckBoxButton.Source = ImageSource.FromFile(isChecked ? "square_ticked.png" : "square.png");
    }
}