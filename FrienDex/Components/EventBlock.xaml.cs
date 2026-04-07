namespace FrienDex.Components;

public partial class EventBlockView : ContentView
{
    public static readonly BindableProperty EventNameProperty =
        BindableProperty.Create(
            nameof(EventName),
            typeof(string),
            typeof(EventBlockView),
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
            typeof(EventBlockView),
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
            typeof(EventBlockView),
            null);

    public string? EventComments
    {
        get => (string?)GetValue(EventCommentsProperty);
        set => SetValue(EventCommentsProperty, value);
    }
    public EventBlockView()
	{
        InitializeComponent();
	}
}