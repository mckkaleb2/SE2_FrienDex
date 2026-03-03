using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrienDex;

public partial class AddContactPage : ContentPage
{
    // This is how we "return" a Contact back to Rolodex
    public TaskCompletionSource<Contact?> ResultTcs { get; } = new();

    public AddContactPage()
    {
        InitializeComponent();
        BindingContext = new AddContactViewModel(this);
    }
}

public class AddContactViewModel : INotifyPropertyChanged
{
    private readonly AddContactPage _page;

    private string name = "";
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(); }
    }

    private string? room;
    public string? Room
    {
        get => room;
        set { room = value; OnPropertyChanged(); }
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddContactViewModel(AddContactPage page)
    {
        _page = page;
        SaveCommand = new Command(OnSave);
        CancelCommand = new Command(OnCancel);
    }

    private void OnSave()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var trimmedName = (Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(trimmedName))
            {
                await _page.DisplayAlertAsync("Missing name", "Please enter a name.", "OK");
                return;
            }

            var newContact = new Contact
            {
                Name = trimmedName,
                Room = string.IsNullOrWhiteSpace(Room) ? null : Room.Trim()
            };

            _page.ResultTcs.TrySetResult(newContact);
            await Shell.Current.GoToAsync("..");
        });
    }

    private void OnCancel()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            _page.ResultTcs.TrySetResult(null);
            await Shell.Current.GoToAsync("..");
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}