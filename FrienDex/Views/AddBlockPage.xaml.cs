using FrienDex.Data.Entities;
using FrienDex.Services;
using System.Collections.ObjectModel;

namespace FrienDex.Views;

[QueryProperty(nameof(PersonId), "PersonId")]
[QueryProperty(nameof(SelectedBlockType), "BlockType")]
public partial class AddBlockPage : ContentPage
{
    public int PersonId { get; set; }
    public string SelectedBlockType { get; set; } = "";

    private readonly IPersonRepo _personRepo;
    private readonly IBlockRepo _blockRepo;

    private Person? _person;
    private bool _isLoadingPeople;

    public ObservableCollection<Person> People { get; } = new();
    public ObservableCollection<string> BlockTypes { get; } = new()
    {
        "Text", "Image", "Date Picker", "Event", "Contact", "Relationship"
    };

    public ObservableCollection<string> ContactTypes { get; } = new()
    {
        "Email", "Phone", "Social Media", "Website", "Address"
    };

    public AddBlockPage(IPersonRepo personRepo, IBlockRepo blockRepo)
    {
        InitializeComponent();
        _personRepo = personRepo;
        _blockRepo = blockRepo;
        BindingContext = this;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (PersonId != 0)
        {
            // Fire and forget, but with internal exception handling
            _ = LoadDataAsync();
        }
    }


    private async Task LoadDataAsync()
    {
        try
        {
            var person = await _personRepo.ReadAsync(PersonId);
            System.Diagnostics.Debug.WriteLine($"[AddBlockPage] After ReadAsync: {DateTime.Now:HH:mm:ss.fff}");

            var allPeople = await _personRepo.ReadAllHierarchyAsync();

            _person = person;

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                People.Clear();
                foreach (var p in allPeople)
                    People.Add(p);
            });

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[AddBlockPage] LoadDataAsync ERROR: {ex}");
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            });
        }
    }




    private async Task LoadPeopleAsync()
    {
        if (_isLoadingPeople) return;
        _isLoadingPeople = true;

        try
        {
            var allPeople = await _personRepo.ReadAllHierarchyAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                People.Clear();
                foreach (var p in allPeople)
                    People.Add(p);
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load people: {ex.Message}", "OK");
        }
        finally
        {
            _isLoadingPeople = false;
        }
    }

    private async void OnBlockTypeChanged(object sender, EventArgs e)
    {
        HideAllContent();

        if (BlockTypePicker.SelectedIndex < 0)
            return;

        string selected = BlockTypePicker.SelectedItem?.ToString() ?? "";

        switch (selected)
        {
            case "Text":
                TextBlockContent.IsVisible = true;
                await Task.Delay(50);
                TextContentEditor.Focus();
                break;

            case "Image":
                ImageBlockContent.IsVisible = true;
                await Task.Delay(50);
                ImageUrlEntry.Focus();
                break;

            case "Date Picker":
                DatePickerBlockContent.IsVisible = true;
                await Task.Delay(50);
                DateTitleEntry.Focus();
                break;

            case "Event":
                EventBlockContent.IsVisible = true;
                await Task.Delay(50);
                EventNameEntry.Focus();
                break;

            case "Contact":
                ContactBlockContent.IsVisible = true;
                break;

            case "Relationship":
                RelationshipBlockContent.IsVisible = true;

                if (People.Count == 0)
                    await LoadPeopleAsync();

                await Task.Delay(50);
                RelationshipNameEntry.Focus();
                break;
        }
    }

    private void HideAllContent()
    {
        TextBlockContent.IsVisible = false;
        ImageBlockContent.IsVisible = false;
        DatePickerBlockContent.IsVisible = false;
        EventBlockContent.IsVisible = false;
        ContactBlockContent.IsVisible = false;
        RelationshipBlockContent.IsVisible = false;
    }

    private async void OnAddBlockClicked(object sender, EventArgs e)
    {
        try
        {
            if (_person?.DexEntry == null)
            {
                await DisplayAlert("Error", "DexEntry not found", "OK");
                return;
            }

            Block? newBlock = BuildBlock();
            if (newBlock == null) return;

            _person.DexEntry.Blocks.Add(newBlock);

            await _blockRepo.CreateAsync(newBlock);
            await _personRepo.UpdateAsync(_person.Id, _person);

            await Shell.Current.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add block: {ex.Message}", "OK");
        }
    }

    private Block? BuildBlock()
    {
        string selected = BlockTypePicker.SelectedItem?.ToString() ?? "";

        switch (selected)
        {
            case "Text":
                if (string.IsNullOrWhiteSpace(TextContentEditor.Text))
                {
                    DisplayAlert("Validation", "Please enter text content", "OK");
                    return null;
                }
                return new TextBlock
                {
                    Content = TextContentEditor.Text,
                    Type = BlockType.TextBlock
                };

            case "Image":
                if (string.IsNullOrWhiteSpace(ImageUrlEntry.Text))
                {
                    DisplayAlert("Validation", "Please enter image URL", "OK");
                    return null;
                }
                return new ImageBlock
                {
                    ImageUrl = ImageUrlEntry.Text,
                    Type = BlockType.ImageBlock
                };

            case "Date Picker":
                if (string.IsNullOrWhiteSpace(DateTitleEntry.Text))
                {
                    DisplayAlert("Validation", "Please enter a title", "OK");
                    return null;
                }
                return new DatePickerBlock
                {
                    DateTitle = DateTitleEntry.Text,
                    DateDescription = DateDescriptionEditor.Text ?? "",
                    SelectedDate = (DateTime)SelectedDatePicker.Date,
                    Type = BlockType.DatePickerBlock
                };

            case "Event":
                if (string.IsNullOrWhiteSpace(EventNameEntry.Text))
                {
                    DisplayAlert("Validation", "Please enter event name", "OK");
                    return null;
                }

                var date = EventDatePicker.Date;
                var time = EventTimePicker.Time;

                if (!date.HasValue)
                {
                    DisplayAlert("Validation", "Please select an event date", "OK");
                    return null;
                }

                return new EventBlock
                {
                    EventName = EventNameEntry.Text,
                    EventDate = date.Value.Add((TimeSpan)time),
                    EventComments = EventCommentsEditor.Text,
                    Type = BlockType.EventBlock
                };

            case "Contact":
                if (ContactTypePicker.SelectedIndex < 0 ||
                    string.IsNullOrWhiteSpace(ContactValueEntry.Text))
                {
                    DisplayAlert("Validation", "Please select type and enter contact value", "OK");
                    return null;
                }
                return new ContactBlock
                {
                    ContactType = ContactTypePicker.SelectedItem?.ToString() ?? "",
                    ContactValue = ContactValueEntry.Text,
                    Type = BlockType.ContactBlock
                };

            case "Relationship":
                if (RelatedPersonPicker.SelectedIndex < 0 ||
                    string.IsNullOrWhiteSpace(RelationshipNameEntry.Text))
                {
                    DisplayAlert("Validation", "Please select person and enter relationship name", "OK");
                    return null;
                }
                return new RelationshipBlock
                {
                    RelationshipName = RelationshipNameEntry.Text,
                    RelationshipDescription = RelationshipDescriptionEditor.Text,
                    RelatedPerson = People[RelatedPersonPicker.SelectedIndex],
                    Type = BlockType.RelationshipBlock
                };

            default:
                DisplayAlert("Error", "Please select a block type", "OK");
                return null;
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
