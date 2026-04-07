namespace FrienDex.Views;

[QueryProperty(nameof(RoomId), "RoomId")]
public partial class ViewRoomPage : ContentPage
{
	public int RoomId { get; set; }
    public ViewRoomPage()
	{
		InitializeComponent();
	}
}