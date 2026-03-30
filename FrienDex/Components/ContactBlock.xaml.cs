namespace FrienDex.Components;

public partial class ContactBlockView : ContentView
{
    public static readonly BindableProperty ContactTypeProperty =
        BindableProperty.Create(
            nameof(ContactType),
            typeof(string),
            typeof(ContactBlockView),
            string.Empty);

    public string ContactType
    {
        get => (string)GetValue(ContactTypeProperty);
        set => SetValue(ContactTypeProperty, value);
    }

    public static readonly BindableProperty ContactValueProperty =
        BindableProperty.Create(
            nameof(ContactValue),
            typeof(string),
            typeof(ContactBlockView),
            string.Empty);

    public string ContactValue
    {
        get => (string)GetValue(ContactValueProperty);
        set => SetValue(ContactValueProperty, value);
    }
    public ContactBlockView()
	{
		InitializeComponent();
	}
}