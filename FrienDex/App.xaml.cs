using Microsoft.Extensions.DependencyInjection;

using FrienDex.Data;

namespace FrienDex
{
    public partial class App : Application
    {
        public static TestItemRepository TestItemRepo { get; private set; }
        public App(TestItemRepository repo)
        {
            // Initialize the TestItemRepository property with the TestItemRespository singleton object
            InitializeComponent();
            TestItemRepo = repo;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}