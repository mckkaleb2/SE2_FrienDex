namespace FrienDex;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
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