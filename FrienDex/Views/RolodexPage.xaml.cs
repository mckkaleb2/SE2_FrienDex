using FrienDex.Data.Entities;
using FrienDex.Services;
using FrienDex.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FrienDex;

public partial class RolodexPage : ContentPage
{
	public ObservableCollection<Person> People { get; set; } = new();
    private readonly IPersonRepo _repo;

    public RolodexPage(IPersonRepo repo)
	{
		InitializeComponent();
		_repo = repo;
		BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

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


        Debug.WriteLine("\n\n\n\t\tRolodexPage is appearing\n\n");

#if DEBUG
		var data = await _repo.ReadAllHierarchyAsync();
		foreach (var person in data)
		{
			Debug.WriteLine($"\n\n\tPerson: {person.ToString()}\n\n");
		}
#endif
	}

	protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Debug.WriteLine("\n\n\n\t\tRolodexPage is disappearing\n\n");
    }

	private async void OnPersonSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is Person selectedPerson)
		{
			// Clear selection
			PeopleCollectionView.SelectedItem = null;

			// Navigate to ViewPerson page with the person's Id
			await Shell.Current.GoToAsync($"{nameof(ViewPersonPage)}?PersonId={selectedPerson.Id}");
		}
	}

	private async void NavigateToCreatePage(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(CreatePersonPage));
	}
}