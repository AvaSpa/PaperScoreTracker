namespace PaperScoreTracker.Controls;

public partial class NumericInput : ContentView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(NumericInput), 0);
    public static readonly BindableProperty IncrementProperty = BindableProperty.Create(nameof(Increment), typeof(int), typeof(NumericInput), 1);

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public int Increment
    {
        get => (int)GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    public NumericInput()
    {
        InitializeComponent();
    }

    private void MinusButton_Clicked(object sender, EventArgs e)
    {
        Value -= Increment;
    }

    private void PlusButton_Clicked(object sender, EventArgs e)
    {
        Value += Increment;
    }
}