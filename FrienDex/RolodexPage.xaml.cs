using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrienDex;

public partial class RolodexPage : ContentPage
{
	public RolodexPage()
	{
		InitializeComponent();
		BindingContext = new RolodexPageViewModel(this);
	}
}

public class RolodexPageViewModel : INotifyPropertyChanged
{
	private readonly Page _page;
	private ObservableCollection<Contact> contacts = new();

	public ObservableCollection<Contact> Contacts
	{
		get => contacts;
		set
		{
			if (contacts != value)
			{
				contacts = value;
				OnPropertyChanged();
			}
		}
	}

	public ICommand AddContactCommand { get; }
	public ICommand ContactSelectedCommand { get; }

	public RolodexPageViewModel(Page page)
	{
		_page = page;
		Contacts = new ObservableCollection<Contact>();
		AddContactCommand = new Command(OnAddContact);
		ContactSelectedCommand = new Command<Contact>(OnContactSelected);

		// TODO: Load contacts from your data source
		LoadContacts();
	}

	private void LoadContacts()
	{
		// Replace with actual data loading
		Contacts.Add(new Contact { Name = "John Doe", Room = "Study Group" });
		Contacts.Add(new Contact { Name = "Jane Smith", Room = "Gaming" });
		Contacts.Add(new Contact { Name = "Bob Johnson", Room = null });
	}

private void OnAddContact()
{
    MainThread.BeginInvokeOnMainThread(async () =>
    {
        var addPage = new AddContactPage();

        // go to the Add page
        await _page.Navigation.PushAsync(addPage);

        // wait for Save/Cancel
        var newContact = await addPage.ResultTcs.Task;

        // if Save was pressed, add it
        if (newContact != null)
        {
            Contacts.Add(newContact);
        }
    });
}

	private void OnContactSelected(Contact contact)
	{
		// TODO: Navigate to contact details
		if (contact != null)
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await _page.DisplayAlertAsync("Contact Selected", $"Selected: {contact.Name}", "OK");
			});
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

public class Contact
{
	public string Name { get; set; } = string.Empty;
	public string? Room { get; set; }
}