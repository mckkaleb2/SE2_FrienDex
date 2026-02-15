using FrienDex.Data;
using Microsoft.Extensions.Logging;

namespace FrienDex
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //NOTE: after the statements that add the MainPage page as a singleton service to the app

            string dbPath = Constants.DatabasePath;
            builder.Services.AddSingleton<TestItemRepository>(s => ActivatorUtilities.CreateInstance<TestItemRepository>(s, dbPath));

#region ServiceRegistration_Pages
            //builder.Services.AddSingleton<TodoListPage>();
            //builder.Services.AddTransient<TodoItemPage>();
#endregion ServiceRegistration_Pages
            builder.Services.AddSingleton<FrienDexDatabase>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
