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
            Routing.RegisterRoute(nameof(ViewPersonPage), typeof(ViewPersonPage));
            Routing.RegisterRoute("editblock", typeof(EditBlockPage));
            Routing.RegisterRoute("addblock", typeof(AddBlockPage)); 
            Routing.RegisterRoute(nameof(ViewRoomPage), typeof(ViewRoomPage));
            Routing.RegisterRoute(nameof(AddMemberRoomPage), typeof(AddMemberRoomPage));    
        }
    }
}
