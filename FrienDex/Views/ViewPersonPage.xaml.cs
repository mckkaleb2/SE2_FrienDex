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

        FirstNameEntry.Text = _person.FirstName;
        LastNameEntry.Text = _person.LastName;
        FavoriteCheckBox.IsChecked = _person.IsFavorite;

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

        BlocksContainer.Children.Clear();
        if (_person.DexEntry != null && _person.DexEntry.Blocks != null && _person.DexEntry.Blocks.Count > 0)
        {
            foreach (var block in _person.DexEntry.Blocks)
            {
                var blockView = CreateBlockView(block);
                if (blockView != null)
                {
                    if (_isEditMode)
                    {
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
        return new TextBlockView
        {
            Text = block.Content
        };
    }

    private View CreateImageBlockView(ImageBlock block)
    {
        return new ImageBlockView
        {
            ImageUrl = block.ImageUrl
        };
    }

    private View CreateDateBlockView(DatePickerBlock block)
    {
        return new DatePickerBlockView
        {
            DateTitle = block.DateTitle,
            DateDescription = block.DateDescription,
            SelectedDate = block.SelectedDate
        };
    }

    private View CreateEventBlockView(EventBlock block)
    {
        return new EventBlockView
        {
            EventName = block.EventName,
            EventDate = block.EventDate,
            EventComments = block.EventComments
        };
    }

    private View CreateRelationshipBlockView(RelationshipBlock block)
    {
        return new RelationshipBlockView
        {
            RelationshipName = block.RelationshipName,
            RelationshipDescription = block.RelationshipDescription,
            RelatedPerson = block.RelatedPerson
        };
    }

    private View CreateContactBlockView(ContactBlock block)
    {
        return new ContactBlockView
        {
            ContactType = block.ContactType,
            ContactValue = block.ContactValue
        };
    }

    private void OnBlockTapped(Block block)
    {
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
        if (_person == null) return;

        _isEditMode = !_isEditMode;

        if (_isEditMode)
        {
            DisplaySection.IsVisible = false;
            EditSection.IsVisible = true;

            EditButton.Text = "💾 Save";
            EditButton.BackgroundColor = Color.FromArgb("#34C759");

            AddBlockButton.IsVisible = true;
        }
        else
        {
            _person.FirstName = FirstNameEntry.Text?.Trim();
            _person.LastName = LastNameEntry.Text?.Trim();
            _person.IsFavorite = FavoriteCheckBox.IsChecked;

            await _personRepo.UpdateAsync(PersonId, _person);

            DisplaySection.IsVisible = true;
            EditSection.IsVisible = false;

            EditButton.Text = "✏️ Edit";
            EditButton.BackgroundColor = Color.FromArgb("#FF6B6B");

            AddBlockButton.IsVisible = false;

            await LoadPersonDetails();
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_person == null) return;

        bool confirm = await DisplayAlert(
            "Delete Person",
            $"Are you sure you want to delete {_person.FullName}?",
            "Yes",
            "No");

        if (!confirm) return;

        await _personRepo.DeleteAsync(PersonId);

        await Shell.Current.DisplayAlert("Deleted", "Person was deleted successfully.", "OK");
        await Shell.Current.GoToAsync("..");
    }

    private async void OnAddBlockClicked(object sender, EventArgs e)
    {
        if (PersonId == 0) return;

        string result = "new";

        if (string.IsNullOrEmpty(result) || result == "Cancel")
            return;

        await Shell.Current.GoToAsync($"addblock?PersonId={PersonId}&BlockType={result}");
    }
}