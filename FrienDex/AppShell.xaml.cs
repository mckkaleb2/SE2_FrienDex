using FrienDex.Views;

namespace FrienDex
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Registered a Route to the Create Person Page because its stupid and needs help.
            Routing.RegisterRoute(nameof(CreatePersonPage), typeof(CreatePersonPage));
        }
    }
}
