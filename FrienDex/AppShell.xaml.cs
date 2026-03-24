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
            Routing.RegisterRoute(nameof(RoomsPage), typeof(RoomsPage));
            Routing.RegisterRoute(nameof(ViewPerson), typeof(ViewPerson));
        }
    }
}
