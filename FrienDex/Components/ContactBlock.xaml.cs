namespace FrienDex.Components;

public partial class ContactBlock : ContentView
{
    public static readonly BindableProperty ContactTypeProperty =
        BindableProperty.Create(
            nameof(ContactType),
            typeof(string),
            typeof(ContactBlock),
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
            typeof(ContactBlock),
            string.Empty);

    public string ContactValue
    {
        get => (string)GetValue(ContactValueProperty);
        set => SetValue(ContactValueProperty, value);
    }
    public ContactBlock()
	{
		InitializeComponent();
	}
}