using FrienDex.Data;
using FrienDex.Services;
using System.Diagnostics;

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

    private async void RoomButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("\n\n\n\t\tRooms button clicked\n\n");
        await Shell.Current.GoToAsync(nameof(RoomsPage));
    }

    private void RolodexButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("\n\n\n\t\tRolodex button clicked\n\n");
        Navigation.PushAsync(new RolodexPage(_repo));
    }

    
    //private void DebugButtonClicked(object sender, TappedEventArgs e)
    //{
    //    Navigation.PushAsync(new DebugPage());
    //}
}