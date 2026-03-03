using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrienDex;

public partial class AddRoomPage : ContentPage
{
    public TaskCompletionSource<DummyRoom?> ResultTcs { get; } = new();

    public AddRoomPage()
    {
        InitializeComponent();
        BindingContext = new AddRoomViewModel(this);
    }
}

public class AddRoomViewModel : INotifyPropertyChanged
{
    private readonly AddRoomPage _page;

    private string name = "";
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(); }
    }

    private string description = "";
    public string Description
    {
        get => description;
        set { description = value; OnPropertyChanged(); }
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddRoomViewModel(AddRoomPage page)
    {
        _page = page;
        SaveCommand = new Command(async () => await OnSaveAsync());
        CancelCommand = new Command(async () => await OnCancelAsync());
    }

    private async Task OnSaveAsync()
    {
        var trimmedName = (Name ?? "").Trim();
        var trimmedDesc = (Description ?? "").Trim();

        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            await _page.DisplayAlertAsync("Missing name", "Please enter a room name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(trimmedDesc))
        {
            await _page.DisplayAlertAsync("Missing description", "Please enter a description.", "OK");
            return;
        }

        var newRoom = new DummyRoom
        {
            Name = trimmedName,
            Description = trimmedDesc
        };

        _page.ResultTcs.TrySetResult(newRoom);
        await _page.Navigation.PopAsync();
    }

    private async Task OnCancelAsync()
    {
        _page.ResultTcs.TrySetResult(null);
        await _page.Navigation.PopAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}