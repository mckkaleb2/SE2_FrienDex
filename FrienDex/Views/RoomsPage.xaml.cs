using FrienDex.Data.Entities;
using FrienDex.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrienDex.Views;
using System.Diagnostics;

namespace FrienDex;

public partial class RoomsPage : ContentPage
{
    public RoomsPage(IRoomRepo roomRepo)
    {
        InitializeComponent();
        BindingContext = new RoomsPageViewModel(this, roomRepo);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // This ensures the state is reset/refreshed every time you navigate back
        if (BindingContext is RoomsPageViewModel viewModel)
        {
            await viewModel.RefreshRooms();
        }
    }
}

public class RoomsPageViewModel : INotifyPropertyChanged
{
    private readonly Page _page;
    private readonly IRoomRepo _roomRepo;
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

    private Room selectedRoom;
    public Room SelectedRoom
    {
        get => selectedRoom;
        set
        {
            if (selectedRoom != value)
            {
                selectedRoom = value;
            }
            OnPropertyChanged();
        }
    }

    public ICommand AddRoomCommand { get; }
    public ICommand RoomSelectedCommand { get; }

    public RoomsPageViewModel(Page page, IRoomRepo roomRepo)
    {
        _page = page;
        _roomRepo = roomRepo;

        Rooms = new ObservableCollection<Room>();

        AddRoomCommand = new Command(OnAddRoom);
        // Parameter is now passed directly from the TapGestureRecognizer
        RoomSelectedCommand = new Command<Room>(OnRoomSelected);
    }

    /// <summary>
    /// Clears the current state and reloads rooms from the repository.
    /// Call this from OnAppearing in the code-behind.
    /// </summary>
    public async Task RefreshRooms()
    {
        var roomsFromRepo = await _roomRepo.ReadAllAsync();

        Rooms.Clear();
        foreach (var room in roomsFromRepo)
        {
            Rooms.Add(room);
        }

        // Reset the selection property to null to ensure a clean state
        SelectedRoom = null;
    }

    private async void OnAddRoom()
    {
        var addPage = new CreateRoomPage(_roomRepo);
        await _page.Navigation.PushAsync(addPage);

        var newRoom = await addPage.ResultTcs.Task;
        if (newRoom != null)
        {
            // The room will also be caught by RefreshRooms when OnAppearing fires,
            // but adding it here provides immediate UI feedback.
            if (!Rooms.Any(r => r.Id == newRoom.Id))
            {
                Rooms.Add(newRoom);
            }
        }
    }

    private async void OnRoomSelected(Room room)
    {
        if (room == null) return;

        Debug.WriteLine($"Navigating to room: {room.Name}");

        // Navigation fires every time because we use TapGesture + SelectionMode="None"
        await Shell.Current.GoToAsync($"{nameof(ViewRoomPage)}?RoomId={room.Id}");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}