namespace FrienDex.Components;

public partial class DatePickerBlock : ContentView
{
    public static readonly BindableProperty DateTitleProperty =
        BindableProperty.Create(
            nameof(DateTitle),
            typeof(string),
            typeof(DatePickerBlock),
            string.Empty);

    public string DateTitle
    {
        get => (string)GetValue(DateTitleProperty);
        set => SetValue(DateTitleProperty, value);
    }

    public static readonly BindableProperty DateDescriptionProperty =
        BindableProperty.Create(
            nameof(DateDescription),
            typeof(string),
            typeof(DatePickerBlock),
            string.Empty);

    public string DateDescription
    {
        get => (string)GetValue(DateDescriptionProperty);
        set => SetValue(DateDescriptionProperty, value);
    }

    public static readonly BindableProperty SelectedDateProperty =
        BindableProperty.Create(
            nameof(SelectedDate),
            typeof(DateTime),
            typeof(DatePickerBlock),
            DateTime.Now);

    public DateTime SelectedDate
    {
        get => (DateTime)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }
    public DatePickerBlock()
	{
		InitializeComponent();
	}
}