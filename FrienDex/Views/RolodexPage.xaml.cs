
//using Android.App.People;
using FrienDex.Data.ViewModels;
using FrienDex.Data.Entities;
using FrienDex.Services;
using FrienDex.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrienDex;

public partial class RolodexPage : ContentPage
{
	//public ReadAllPeopleVM _viewModel;

	public ObservableCollection<Person> People { get; set; } = new();

    public RolodexPage(IPersonRepo repo)
	{
		InitializeComponent();

		_repo = repo;
		BindingContext = this;
		//BindingContext = new RolodexPageViewModel(this);
		//_repo = repo;

		// ReadAll stuff (unsure if it will work yet)
		//BindingContext = vm;
		//_viewModel = vm;
	}

	private readonly IPersonRepo _repo;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		// Put your C# code here that needs to run when the page appears.
		var people = await _repo.ReadAllAsync();

		People.Clear();
        // Favorite Priority Queue
        People.Add(new Person { FirstName = "Jonathan", LastName = "Motherius", IsFavorite = true });
        foreach (var person in people)
		{
            if (person.IsFavorite == true)
			{
                People.Add(person);
            }
        }

        // Everybody Else
        foreach (var person in people)
        {
            if (person.IsFavorite == false)
            {
                People.Add(person);
            }
        }


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

	private async void NavigateToCreatePage(object sender, EventArgs e)
	{
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync(nameof(CreatePersonPage));
        });
        //await Navigation.PushAsync(new CreatePersonPage());
    }
}

// Change all instances of 'Contact' class reference to 'Person'.

public class RolodexPageViewModel : INotifyPropertyChanged
{
	private readonly Page _page;
	private readonly PersonRepo _repo;
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

	public RolodexPageViewModel(Page page)//, PersonRepo repo)
	{
		_page = page;
		//_repo = repo;
		
		Contacts = new ObservableCollection<Contact>();
		AddContactCommand = new Command(OnAddContact);
		ContactSelectedCommand = new Command<Contact>(OnContactSelected);

		// TODO: Load contacts from your data source
		LoadContacts();
    }

	// Not sure why this exists yet. It was generated to allow RolodexPageViewModel to accept multiple parameter.
	//public RolodexPageViewModel(RolodexPage rolodexPage)
	//{
	//}

	private async Task LoadContacts()
	{ 
		// Replace with actual data loading
		Contacts.Add(new Contact { Name = "John Doe", Room = "Study Group" });
		Contacts.Add(new Contact { Name = "Jane Smith", Room = "Gaming" });
		Contacts.Add(new Contact { Name = "Bob Johnson", Room = null });

		//await _repo.ReadAllHierarchyAsync();
	}

	//private void OnAddContact(object sender, EventArgs e)
	//{
	//	Navigation.PushAsync(new CreatePersonPage);
	//}

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