using FrienDex.Data.Entities;
using FrienDex.Services;
using System.Collections.ObjectModel;

namespace FrienDex.Views;

[QueryProperty(nameof(RoomId), "RoomId")]
public partial class RemoveMemberRoomPage : ContentPage
{
    private readonly IRoomRepo _roomRepo;
    private readonly IPersonRepo _personRepo;
    public int RoomId { get; set; }
    public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();
    public ObservableCollection<Person> SelectedPeople { get; set; } = new ObservableCollection<Person>();
    public RemoveMemberRoomPage(IRoomRepo roomRepo, IPersonRepo personRepo)
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
            var room = await _roomRepo.ReadAsync(RoomId);
            foreach (var person in room!.People)
            {
                People.Add(person);
            }
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
    private async void OnRemoveButtonClicked(object sender, EventArgs e)
    {
        if (SelectedPeople.Count == 0) return;

        try
        {
            var room = await _roomRepo.ReadAsync(RoomId);
            foreach (var person in SelectedPeople)
            {
                // 1. Remove the person from the room's People collection
                if (room != null)
                {
                    room.People.RemoveAll(p => p.Id == person.Id);
                    // 2. Update the person's Rooms collection
                    var personToUpdate = await _personRepo.ReadAsync(person.Id);
                    if (personToUpdate != null)
                    {
                        personToUpdate.Rooms.RemoveAll(r => r.Id == RoomId);
                        await _personRepo.UpdateAsync(personToUpdate.Id, personToUpdate);
                    }
                }
            }

            // 4. Update the room
            await _roomRepo.UpdateAsync(RoomId, room!);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}