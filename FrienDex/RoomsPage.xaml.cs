using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrienDex;

public partial class RoomsPage : ContentPage
{
	public RoomsPage()
	{
		InitializeComponent();
		BindingContext = new RoomsPageViewModel(this);
	}
}

public class RoomsPageViewModel : INotifyPropertyChanged
{
	private readonly Page _page;
	private ObservableCollection<Room> rooms = new();

	public ObservableCollection<Room> Rooms
	{
		get => rooms;
		set
		{
			if (rooms != value)
			{
				rooms = value;
				OnPropertyChanged();
			}
		}
	}

	public ICommand AddRoomCommand { get; }
	public ICommand RoomSelectedCommand { get; }

	public RoomsPageViewModel(Page page)
	{
		_page = page;
		Rooms = new ObservableCollection<Room>();
		AddRoomCommand = new Command(OnAddRoom);
		RoomSelectedCommand = new Command<Room>(OnRoomSelected);

		// TODO: Load rooms from your data source
		LoadRooms();
	}
    /// <summary>
    /// Fetches the list of rooms from the data source and populates the Rooms collection.
    /// </summary>
    private void LoadRooms()
	{
		// Replace with actual data loading
		Rooms.Add(new Room { Name = "Study Group", Description = "Computer Science" });
		Rooms.Add(new Room { Name = "Gaming", Description = "Multiplayer games" });
	}

	private void OnAddRoom()
	{
		// TODO: Navigate to add room page or show dialog
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			await _page.DisplayAlertAsync("Add Room", "Add room functionality coming soon", "OK");
		});
	}

	private void OnRoomSelected(Room room)
	{
		// TODO: Navigate to room details
		if (room != null)
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await _page.DisplayAlertAsync("Room Selected", $"Selected: {room.Name}", "OK");
			});
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

public class Room
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
}