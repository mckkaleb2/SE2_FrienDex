using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrienDex;

public partial class DebugPage : ContentPage
{
    public DebugPage()
    {
        InitializeComponent();
        BindingContext = new DebugPageViewModel(this);
    }


    public class DebugPageViewModel : INotifyPropertyChanged
    {
        private readonly Page _page;
        public DebugPageViewModel(Page page)
        {
            _page = page;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

}