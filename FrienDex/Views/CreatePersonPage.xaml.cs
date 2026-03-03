using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FrienDex.Views;

public partial class CreatePersonPage : ContentPage
{
	public CreatePersonPage()
	{
		InitializeComponent();
		//BindingContext = new CreatePersonPageViewModel(this);
	}
}

//public class CreatePersonPageViewModel : INotifyPropertyChanged
//{
//    private readonly Page _page;
    
//    public CreatePersonPageViewModel(Page page)
//    {
//        _page = page;

//    }
//}