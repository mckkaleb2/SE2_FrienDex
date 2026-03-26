namespace FrienDex.Components;

public partial class EventBlock : ContentView
{
    public static readonly BindableProperty EventNameProperty =
        BindableProperty.Create(
            nameof(EventName),
            typeof(string),
            typeof(EventBlock),
            string.Empty);

    public string EventName
    {
        get => (string)GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
    }

    public static readonly BindableProperty EventDateProperty =
        BindableProperty.Create(
            nameof(EventDate),
            typeof(DateTime),
            typeof(EventBlock),
            DateTime.Now);

    public DateTime EventDate
    {
        get => (DateTime)GetValue(EventDateProperty);
        set => SetValue(EventDateProperty, value);
    }

    public static readonly BindableProperty EventCommentsProperty =
        BindableProperty.Create(
            nameof(EventComments),
            typeof(string),
            typeof(EventBlock),
            null);

    public string? EventComments
    {
        get => (string?)GetValue(EventCommentsProperty);
        set => SetValue(EventCommentsProperty, value);
    }
    public EventBlock()
	{
		InitializeComponent();
	}
}