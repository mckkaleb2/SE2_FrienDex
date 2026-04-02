using FrienDex.Data.Entities;
using FrienDex.Services;
using FrienDex.Components;
using Microsoft.Maui.Controls.Shapes;

namespace FrienDex.Views;

[QueryProperty(nameof(PersonId), "PersonId")]
public partial class ViewPersonPage : ContentPage
{
    public int PersonId { get; set; }
    private readonly IPersonRepo _personRepo;
    private readonly IBlockRepo _blockRepo;
    private Person? _person;
    private bool _isEditMode = false;
    
    public ViewPersonPage(IPersonRepo personRepo, IBlockRepo blockRepo)
	{
		InitializeComponent();
        _personRepo = personRepo;
        _blockRepo = blockRepo;
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
            ShowErrorState();
        }
    }

    private void PopulateUI()
    {
        if (_person == null) return;

        NameLabel.Text = $"{_person.FirstName} {_person.LastName}".Trim();

        if (_person.IsFavorite)
        {
            FavoriteIndicator.Text = "⭐";
            FavoriteLabel.Text = "Favorite";
        }
        else
        {
            FavoriteIndicator.Text = "";
            FavoriteLabel.Text = "";
        }

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

        // Render blocks dynamically
        BlocksContainer.Children.Clear();
        if (_person.DexEntry != null && _person.DexEntry.Blocks != null && _person.DexEntry.Blocks.Count > 0)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"✅ Blocks loaded: {_person.DexEntry.Blocks.Count} blocks found");
#endif
            
            foreach (var block in _person.DexEntry.Blocks)
            {
                var blockView = CreateBlockView(block);
                if (blockView != null)
                {
                    // Add spacing between blocks instead of border wrapping
                    if (_isEditMode)
                    {
                        // Add tap gesture for edit mode
                        var tapGesture = new TapGestureRecognizer();
                        tapGesture.Tapped += (s, e) => OnBlockTapped(block);
                        blockView.GestureRecognizers.Add(tapGesture);
                    }

                    BlocksContainer.Children.Add(blockView);
                }
            }
        }
        else
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"⚠️ No blocks found for this person");
#endif
            BlocksContainer.Children.Add(new Label
            {
                Text = "No blocks yet",
                FontSize = 12,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center
            });
        }
    }

    private View? CreateBlockView(Block block)
    {
        return block switch
        {
            TextBlock textBlock => CreateTextBlockView(textBlock),
            ImageBlock imageBlock => CreateImageBlockView(imageBlock),
            DatePickerBlock dateBlock => CreateDateBlockView(dateBlock),
            EventBlock eventBlock => CreateEventBlockView(eventBlock),
            RelationshipBlock relationshipBlock => CreateRelationshipBlockView(relationshipBlock),
            ContactBlock contactBlock => CreateContactBlockView(contactBlock),
            _ => null
        };
    }

    private View CreateTextBlockView(TextBlock block)
    {
        var textBlockView = new TextBlockView
        {
            Text = block.Content
        };
        return textBlockView;
    }

    private View CreateImageBlockView(ImageBlock block)
    {
        var imageBlockView = new ImageBlockView
        {
            ImageUrl = block.ImageUrl
        };
        return imageBlockView;
    }

    private View CreateDateBlockView(DatePickerBlock block)
    {
        var dateBlockView = new DatePickerBlockView
        {
            DateTitle = block.DateTitle,
            DateDescription = block.DateDescription,
            SelectedDate = block.SelectedDate
        };
        return dateBlockView;
    }

    private View CreateEventBlockView(EventBlock block)
    {
        var eventBlockView = new EventBlockView
        {
            EventName = block.EventName,
            EventDate = block.EventDate,
            EventComments = block.EventComments
        };
        return eventBlockView;
    }

    private View CreateRelationshipBlockView(RelationshipBlock block)
    {
        var relationshipBlockView = new RelationshipBlockView
        {
            RelationshipName = block.RelationshipName,
            RelationshipDescription = block.RelationshipDescription,
            RelatedPerson = block.RelatedPerson
        };
        return relationshipBlockView;
    }

    private View CreateContactBlockView(ContactBlock block)
    {
        var contactBlockView = new ContactBlockView
        {
            ContactType = block.ContactType,
            ContactValue = block.ContactValue
        };
        return contactBlockView;
    }

    private void OnBlockTapped(Block block)
    {
        // Handle block edit - navigate to edit page
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync($"editblock?BlockId={block.Id}&PersonId={PersonId}");
        });
    }

    private void ShowErrorState()
    {
        NameLabel.Text = "Error loading details";
        NameLabel.TextColor = Colors.Red;
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        _isEditMode = !_isEditMode;

        if (_isEditMode)
        {
            // Switch to edit mode
            var editButton = (Button)sender;
            editButton.Text = "✨ Done";
            editButton.BackgroundColor = Color.FromArgb("#34C759");

            // Show add block button if it exists in XAML
            if (AddBlockButton != null)
            {
                AddBlockButton.IsVisible = true;
            }
        }
        else
        {
            // Exit edit mode
            var editButton = (Button)sender;
            editButton.Text = "✏️ Edit";
            editButton.BackgroundColor = Color.FromArgb("#FF6B6B");

            // Hide add block button
            if (AddBlockButton != null)
            {
                AddBlockButton.IsVisible = false;
            }

            // Refresh UI to save changes
            if (_person != null)
            {
                await _personRepo.UpdateAsync(PersonId, _person);
                PopulateUI();
            }
        }
    }

    private async void OnAddBlockClicked(object sender, EventArgs e)
    {
        if (PersonId == 0) return;
        string result = "new";

        if (string.IsNullOrEmpty(result) || result == "Cancel")
            return;

        // Navigate to AddBlockPage with selected block type
        await Shell.Current.GoToAsync($"addblock?PersonId={PersonId}&BlockType={result}");
    }
}