using FrienDex.Data.Entities;
using FrienDex.Services;
using System.Collections.ObjectModel;

namespace FrienDex.Views;

[QueryProperty(nameof(BlockId), "BlockId")]
[QueryProperty(nameof(PersonId), "PersonId")]
public partial class EditBlockPage : ContentPage
{
    public int BlockId { get; set; }
    public int PersonId { get; set; }

    private readonly IPersonRepo _personRepo;
    private readonly IBlockRepo _blockRepo;
    private Block? _block;
    private Person? _person;
    private ObservableCollection<Person> _people;
    private ObservableCollection<string> _contactTypes;

    public EditBlockPage(IPersonRepo personRepo, IBlockRepo blockRepo)
    {
        InitializeComponent();
        _personRepo = personRepo;
        _blockRepo = blockRepo;
        _people = new ObservableCollection<Person>();
        _contactTypes = new ObservableCollection<string>
        {
            "Email",
            "Phone",
            "Social Media",
            "Website",
            "Address"
        };

        BindingContext = this;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BlockId != 0 && PersonId != 0)
        {
            _ = LoadData();
        }
    }

    private async Task LoadData()
    {
        try
        {
            _block = await _blockRepo.ReadAsync(BlockId);
            _person = await _personRepo.ReadAsync(PersonId);

            if (_block != null)
            {
                PopulateUI();
            }

            // Load all people for relationship picker
            var allPeople = await _personRepo.ReadAllAsync();
            _people.Clear();
            foreach (var person in allPeople)
            {
                _people.Add(person);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Failed to load block: {ex.Message}", "OK");
        }
    }

    public ObservableCollection<string> ContactTypes => _contactTypes;
    public ObservableCollection<Person> People => _people;

    private void PopulateUI()
    {
        if (_block == null) return;

        BlockTypeLabel.Text = _block.Type.ToString();

        HideAllEditContent();

        switch (_block)
        {
            case TextBlock textBlock:
                TextBlockEditContent.IsVisible = true;
                TextContentEditEditor.Text = textBlock.Content;
                break;

            case ImageBlock imageBlock:
                ImageBlockEditContent.IsVisible = true;
                ImageUrlEditEntry.Text = imageBlock.ImageUrl;
                break;

            case DatePickerBlock datePickerBlock:
                DatePickerBlockEditContent.IsVisible = true;
                DateTitleEditEntry.Text = datePickerBlock.DateTitle;
                DateDescriptionEditEditor.Text = datePickerBlock.DateDescription;
                SelectedDateEditPicker.Date = datePickerBlock.SelectedDate;
                break;

            case EventBlock eventBlock:
                EventBlockEditContent.IsVisible = true;
                EventNameEditEntry.Text = eventBlock.EventName;
                EventDateEditPicker.Date = eventBlock.EventDate.Date;
                EventTimeEditPicker.Time = eventBlock.EventDate.TimeOfDay;
                EventCommentsEditEditor.Text = eventBlock.EventComments ?? "";
                break;

            case ContactBlock contactBlock:
                ContactBlockEditContent.IsVisible = true;
                ContactTypeEditPicker.SelectedItem = contactBlock.ContactType;
                ContactValueEditEntry.Text = contactBlock.ContactValue;
                break;

            case RelationshipBlock relationshipBlock:
                RelationshipBlockEditContent.IsVisible = true;
                RelationshipNameEditEntry.Text = relationshipBlock.RelationshipName;
                RelationshipDescriptionEditEditor.Text = relationshipBlock.RelationshipDescription ?? "";
                if (relationshipBlock.RelatedPerson != null)
                {
                    RelatedPersonEditPicker.SelectedItem = relationshipBlock.RelatedPerson;
                }
                break;
        }
    }

    private void HideAllEditContent()
    {
        TextBlockEditContent.IsVisible = false;
        ImageBlockEditContent.IsVisible = false;
        DatePickerBlockEditContent.IsVisible = false;
        EventBlockEditContent.IsVisible = false;
        ContactBlockEditContent.IsVisible = false;
        RelationshipBlockEditContent.IsVisible = false;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (_block == null) return;

            switch (_block)
            {
                case TextBlock textBlock:
                    if (string.IsNullOrWhiteSpace(TextContentEditEditor.Text))
                    {
                        await DisplayAlertAsync("Validation", "Please enter text content", "OK");
                        return;
                    }
                    textBlock.Content = TextContentEditEditor.Text;
                    break;

                case ImageBlock imageBlock:
                    if (string.IsNullOrWhiteSpace(ImageUrlEditEntry.Text))
                    {
                        await DisplayAlertAsync("Validation", "Please enter image URL", "OK");
                        return;
                    }
                    imageBlock.ImageUrl = ImageUrlEditEntry.Text;
                    break;

                case DatePickerBlock datePickerBlock:
                    if (string.IsNullOrWhiteSpace(DateTitleEditEntry.Text))
                    {
                        await DisplayAlertAsync("Validation", "Please enter a title", "OK");
                        return;
                    }
                    datePickerBlock.DateTitle = DateTitleEditEntry.Text;
                    datePickerBlock.DateDescription = DateDescriptionEditEditor.Text ?? "";
                    datePickerBlock.SelectedDate = (DateTime)SelectedDateEditPicker.Date;
                    break;

                case EventBlock eventBlock:
                    if (string.IsNullOrWhiteSpace(EventNameEditEntry.Text))
                    {
                        await DisplayAlertAsync("Validation", "Please enter event name", "OK");
                        return;
                    }
                    eventBlock.EventName = EventNameEditEntry.Text;

                    // unwrap nullable DateTime and TimeSpan safely
                    var date = EventDateEditPicker.Date ?? DateTime.Now.Date;
                    var time = EventTimeEditPicker.Time ?? TimeSpan.Zero;
                    eventBlock.EventDate = date.Add(time);

                    eventBlock.EventComments = EventCommentsEditEditor.Text;
                    break;

                case ContactBlock contactBlock:
                    if (ContactTypeEditPicker.SelectedIndex < 0 || string.IsNullOrWhiteSpace(ContactValueEditEntry.Text))
                    {
                        await DisplayAlertAsync("Validation", "Please select type and enter contact value", "OK");
                        return;
                    }
                    contactBlock.ContactType = ContactTypeEditPicker.SelectedItem?.ToString() ?? "";
                    contactBlock.ContactValue = ContactValueEditEntry.Text;
                    break;

                case RelationshipBlock relationshipBlock:
                    if (RelatedPersonEditPicker.SelectedIndex < 0 || string.IsNullOrWhiteSpace(RelationshipNameEditEntry.Text))
                    {
                        await DisplayAlertAsync("Validation", "Please select person and enter relationship name", "OK");
                        return;
                    }
                    relationshipBlock.RelationshipName = RelationshipNameEditEntry.Text;
                    relationshipBlock.RelationshipDescription = RelationshipDescriptionEditEditor.Text;
                    
                    // Re-fetch the person to ensure it's tracked by the DbContext
                    var selectedPerson = _people[RelatedPersonEditPicker.SelectedIndex];
                    relationshipBlock.RelatedPerson = await _personRepo.ReadAsync(selectedPerson.Id);
                    break;
            }

            await _blockRepo.UpdateAsync(_block);
            await Shell.Current.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Failed to save block: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Confirm", "Delete this block?", "Yes", "No");
        if (confirm && _block != null)
        {
            try
            {
                await _blockRepo.DeleteAsync(_block.Id);
                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Failed to delete block: {ex.Message}", "OK");
            }
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }
}