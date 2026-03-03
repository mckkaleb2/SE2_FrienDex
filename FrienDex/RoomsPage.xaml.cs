using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrienDex.Data.Entities;

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
	private ObservableCollection<DummyRoom> rooms = new();

	public ObservableCollection<DummyRoom> Rooms
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
		Rooms = new ObservableCollection<DummyRoom>();
		AddRoomCommand = new Command(OnAddRoom);
		RoomSelectedCommand = new Command<DummyRoom>(OnRoomSelected);

		// TODO: Load rooms from your data source
		LoadRooms();
	}
    /// <summary>
    /// Fetches the list of rooms from the data source and populates the Rooms collection.
    /// </summary>
    private void LoadRooms()
	{
		// Replace with actual data loading
		Rooms.Add(new DummyRoom { Name = "Study Group", Description = "Computer Science" });
		Rooms.Add(new DummyRoom { Name = "Gaming", Description = "Multiplayer games" });
	}

	private void OnAddRoom()
{
    MainThread.BeginInvokeOnMainThread(async () =>
    {
        var addPage = new AddRoomPage();
        await _page.Navigation.PushAsync(addPage);

        var newRoom = await addPage.ResultTcs.Task;
        if (newRoom != null)
        {
            Rooms.Add(newRoom);
        }
    });
}

	private void OnRoomSelected(DummyRoom room)
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

public class DummyRoom
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
}