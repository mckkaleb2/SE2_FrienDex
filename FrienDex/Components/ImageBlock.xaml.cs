namespace FrienDex.Components;

public partial class ImageBlock : ContentView
{
    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(
            nameof(ImageUrl),
            typeof(string),
            typeof(ImageBlock),
            string.Empty);

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public ImageBlock()
    {
        InitializeComponent();
    }
}