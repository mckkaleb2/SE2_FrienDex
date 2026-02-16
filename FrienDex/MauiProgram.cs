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


#region ServiceRegistration_Pages
            //builder.Services.AddSingleton<TodoListPage>();
            //builder.Services.AddTransient<TodoItemPage>();
#endregion ServiceRegistration_Pages

            string dbPath = Constants.DatabasePath;
            
            // NOTE: Use FrienDexDatabase as a template to create Repository.cs files for other entities! It contains basic Async CRRUD functions as well as logging!
            //builder.Services.AddSingleton<FrienDexDatabase>();
            builder.Services.AddSingleton<TestItemRepository>(s => ActivatorUtilities.CreateInstance<TestItemRepository>(s, dbPath));



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
