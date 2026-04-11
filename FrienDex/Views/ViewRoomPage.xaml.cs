using FrienDex.Services;
using System.Collections.ObjectModel;
using FrienDex.Data.Entities;
namespace FrienDex.Views;

[QueryProperty(nameof(RoomId), "RoomId")]
public partial class ViewRoomPage : ContentPage
{
	private readonly IRoomRepo _roomRepo;
    public int RoomId { get; set; }
	public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();
    public ViewRoomPage(IRoomRepo roomRepo)
	{
		_roomRepo = roomRepo;
        InitializeComponent();
		BindingContext = this;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
		if (RoomId != 0)
		{
			People.Clear();
			_ = LoadRoomDetails();
		}
	}
	private async Task LoadRoomDetails()
	{
		try
		{
			var room = await _roomRepo.ReadAsync(RoomId);
			RoomNameLabel.Text = room!.Name;
			RoomDescriptionLabel.Text = room!.Description;
			foreach (var person in room.People)
			{
				People.Add(person);
            }
        }
		catch (Exception)
		{
			ShowErrorState();
		}
	}
	private void ShowErrorState()
	{
		RoomNameLabel.Text = "Error loading room";
		RoomDescriptionLabel.Text = "";
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"AddMemberRoomPage?RoomId={RoomId}");
    }
	private async void OnRemoveButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync($"RemoveMemberRoomPage?RoomId={RoomId}");
    }
}