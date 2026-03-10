using FrienDex.Data.Entities;
using FrienDex.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrienDex.Views;

namespace FrienDex;

public partial class RoomsPage : ContentPage
{
    public RoomsPage(IRoomRepo roomRepo)
    {
        InitializeComponent();
        BindingContext = new RoomsPageViewModel(this, roomRepo);
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

    public ICommand AddRoomCommand { get; }
    public ICommand RoomSelectedCommand { get; }

    public RoomsPageViewModel(Page page, IRoomRepo roomRepo)
    {
        _page = page;
        _roomRepo = roomRepo;
        Rooms = new ObservableCollection<Room>();
        AddRoomCommand = new Command(OnAddRoom);
        RoomSelectedCommand = new Command<Room>(OnRoomSelected);

        LoadRooms();
    }
    
    /// <summary>
    /// Fetches the list of rooms from the data source and populates the Rooms collection.
    /// </summary>
    private async void LoadRooms()
    {
        var roomsFromRepo = await _roomRepo.ReadAllAsync();
        foreach (var room in roomsFromRepo)
        {
            Rooms.Add(room);
        }
    }

    private async void OnAddRoom()
    {
        var addPage = new CreateRoomPage(_roomRepo);
        await _page.Navigation.PushAsync(addPage);

        var newRoom = await addPage.ResultTcs.Task;
        if (newRoom != null)
        {
            Rooms.Add(newRoom);
        }
    }

    private void OnRoomSelected(Room room)
    {
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
