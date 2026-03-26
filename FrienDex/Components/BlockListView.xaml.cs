using FrienDex.Data.Entities;

namespace FrienDex.Components;

public partial class BlockListView : ContentView
{
    public static readonly BindableProperty BlocksProperty =
        BindableProperty.Create(
            nameof(Blocks),
            typeof(IEnumerable<Block>),
            typeof(BlockListView),
            default(IEnumerable<Block>));

    public IEnumerable<Block> Blocks
    {
        get => (IEnumerable<Block>)GetValue(BlocksProperty);
        set => SetValue(BlocksProperty, value);
    }
    public BlockListView()
	{
		InitializeComponent();
	}
}