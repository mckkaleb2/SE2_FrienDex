using FrienDex.Data;
using FrienDex.Services;

namespace FrienDex;

public partial class MainPage : ContentPage
{
    private readonly DexContext _db;
    private readonly IPersonRepo _repo;

    public MainPage(DexContext db, IPersonRepo repo)
    {
        InitializeComponent();
        _db = db;
        _repo = repo;
    }

    private void RoomButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RoomsPage());
    }

    private void RolodexButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RolodexPage(_repo));
    }

    private void DebugButtonClicked(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new DebugPage());
    }
}