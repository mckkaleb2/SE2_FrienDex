using FrienDex.Data.Entities;
using FrienDex.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace FrienDex.Views;

[QueryProperty(nameof(PersonId), "PersonId")]
public partial class ViewPersonPage : ContentPage
{
	public int PersonId { get; set; }
    private readonly IPersonRepo _personRepo;
    private Person? _person;
    
    public ViewPersonPage(IPersonRepo personRepo)
	{
		InitializeComponent();
        _personRepo = personRepo;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (PersonId != 0)
        {
            _ = LoadPersonDetails();
        }
    }

    private async Task LoadPersonDetails()
    {
        try
        {
            _person = await _personRepo.ReadAsync(PersonId);
            PopulateUI();
        }
        catch (Exception)
        {
            // Optionally show error state in UI
            ShowErrorState();
        }
    }

    private void PopulateUI()
    {
        if (_person == null) return;

        // Set name
        NameLabel.Text = $"{_person.FirstName} {_person.LastName}".Trim();

        // Set favorite indicator
        if (_person.IsFavorite.HasValue && _person.IsFavorite.Value)
        {
            FavoriteIndicator.Text = "⭐";
            FavoriteLabel.Text = "Favorite";
        }
        else
        {
            FavoriteIndicator.Text = "";
            FavoriteLabel.Text = "";
        }

        // Populate rooms pills
        RoomsPillContainer.Children.Clear();
        if (_person.Rooms != null && _person.Rooms.Count > 0)
        {
            foreach (var room in _person.Rooms)
            {
                var pill = new Border
                {
                    Stroke = Color.FromArgb("#007AFF"),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 16 },
                    Padding = new Thickness(12, 6),
                    BackgroundColor = Color.FromArgb("#F0F8FF"),
                    Content = new Label
                    {
                        Text = room.Name ?? "Room",
                        FontSize = 12,
                        TextColor = Color.FromArgb("#007AFF"),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    }
                };
                RoomsPillContainer.Children.Add(pill);
            }
        }
        else
        {
            RoomsPillContainer.Children.Add(new Label
            {
                Text = "No rooms assigned",
                FontSize = 12,
                TextColor = Colors.Gray
            });
        }

        // Set DexEntry content
        if (_person.DexEntry != null)
        {
            DexEntryContent.Text = _person.DexEntry.ToString();
        }
        else
        {
            DexEntryContent.Text = "No additional details available";
            DexEntryContent.TextColor = Colors.Gray;
        }
    }

    private void ShowErrorState()
    {
        NameLabel.Text = "Error loading details";
        NameLabel.TextColor = Colors.Red;
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        return;
    }
}