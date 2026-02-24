using Microsoft.EntityFrameworkCore;
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

    private void RoomButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RoomsPage());
    }

    private void RolodexButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RolodexPage());
    }
}