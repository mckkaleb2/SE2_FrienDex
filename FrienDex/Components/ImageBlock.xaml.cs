namespace FrienDex.Components;

public partial class ImageBlockView : ContentView
{
    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(
            nameof(ImageUrl),
            typeof(string),
            typeof(ImageBlockView),
            string.Empty);

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public ImageBlockView()
    {
        InitializeComponent();
    }
}