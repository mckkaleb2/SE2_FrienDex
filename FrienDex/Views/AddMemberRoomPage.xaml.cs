using FrienDex.Data.Entities;
using FrienDex.Services;
using System.Collections.ObjectModel;

namespace FrienDex.Views;

[QueryProperty(nameof(RoomId), "RoomId")]
public partial class AddMemberRoomPage : ContentPage
{
	private readonly IRoomRepo _roomRepo;
    private readonly IPersonRepo _personRepo;
    public int RoomId { get; set; }
    public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();
    public ObservableCollection<Person> SelectedPeople { get; set; } = new ObservableCollection<Person>();
    public AddMemberRoomPage(IRoomRepo roomRepo, IPersonRepo personRepo)
	{
		_roomRepo = roomRepo;
        _personRepo = personRepo;
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (RoomId != 0)
        {
            _ = LoadRolodex();
        }
    }

    private async Task LoadRolodex()
    {
        try
        {
            // 1. Run the heavy data lifting on a background thread
            var filteredPeople = await Task.Run(async () =>
            {
                var allPeople = await _personRepo.ReadAllAsync();
                var room = await _roomRepo.ReadAsync(RoomId);
                var currentMemberIds = room?.People.Select(p => p.Id).ToHashSet() ?? new HashSet<int>();

                // Filter using LINQ (faster and cleaner)
                return allPeople.Where(p => !currentMemberIds.Contains(p.Id)).ToList();
            });

            // 2. Update the UI Collection once the data is ready
            MainThread.BeginInvokeOnMainThread(() =>
            {
                People.Clear();
                foreach (var person in filteredPeople)
                {
                    People.Add(person);
                }
            });
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Failed to load rolodex.", "OK");
        }
    }
    private void PeopleCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (Person person in e.CurrentSelection)
        {
            if (!SelectedPeople.Contains(person))
            {
                SelectedPeople.Add(person);
            }
        }
        foreach (Person person in e.PreviousSelection)
        {
            if (!e.CurrentSelection.Contains(person))
            {
                SelectedPeople.Remove(person);
            }
        }
    }
    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        if (SelectedPeople.Count == 0) return;

        try
        {
            // 1. Fetch the room including existing members
            var room = await _roomRepo.ReadAsync(RoomId);
            if (room == null) return;

            // 2. Clear tracking issues by getting the IDs we want to add
            var selectedIds = SelectedPeople.Select(p => p.Id).ToList();

            // 3. IMPORTANT: Fetch the Person entities specifically through the 
            // same Repo/Context that just loaded the Room.
            foreach (var personId in selectedIds)
            {
                if (!room.People.Any(p => p.Id == personId))
                {
                    // Re-fetch the person within this operation's scope 
                    // OR use a Repo method that handles Attachments.
                    var personToAdd = await _personRepo.ReadAsync(personId);
                    if (personToAdd != null)
                    {
                        room.People.Add(personToAdd);
                    }
                }
            }

            // 4. Update the room
            await _roomRepo.UpdateAsync(RoomId, room);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}