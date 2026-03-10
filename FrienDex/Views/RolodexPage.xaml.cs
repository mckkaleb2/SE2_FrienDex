
using FrienDex.Services;
using FrienDex.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrienDex;

public partial class RolodexPage : ContentPage
{
	public RolodexPage(IPersonRepo repo)
	{
		InitializeComponent();
		BindingContext = new RolodexPageViewModel(this);
		_repo = repo;
	}

	private readonly IPersonRepo _repo;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Put your C# code here that needs to run when the page appears.
        System.Diagnostics.Debug.WriteLine("\n\n\n\t\tRolodexPage is appearing\n\n");

#if DEBUG
        var data = await _repo.ReadAllHierarchyAsync();
		foreach (var person in data)
		{
			System.Diagnostics.Debug.WriteLine($"\n\n\tPerson: {person.ToString()}\n\n");
        }
#endif

        // Example tasks:
        // * Refreshing data from a service
        // * Updating UI elements
        // * Starting animations
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Put code here that needs to run when the page disappears (e.g., stopping animations).
        System.Diagnostics.Debug.WriteLine("\n\n\n\t\tRolodexPage is disappearing\n\n");
    }

}

// Change all instances of 'Contact' class reference to 'Person'.

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

	//  private void OnAddContact(object sender, EventArgs e)
	//  {
	//Navigation.PushAsync(new CreatePersonPage);
	//  }

	private void OnAddContact()
	{
		// TODO: Navigate to add contact page or show dialog
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			await Shell.Current.GoToAsync(nameof(CreatePersonPage));
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