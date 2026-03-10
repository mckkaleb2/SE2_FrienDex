using FrienDex.Data;

namespace FrienDex;

public partial class MainPage : ContentPage
{
    private readonly DexContext _db;
    public MainPage(DexContext db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void RoomButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RoomsPage));
    }

    private void RolodexButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RolodexPage());
    }

    private void DebugButtonClicked(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new DebugPage());
    }
}