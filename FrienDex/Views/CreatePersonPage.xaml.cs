using FrienDex.Data.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FrienDex.Views;

public partial class CreatePersonPage : ContentPage
{
	public CreatePersonPage(CreatePersonVM vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}