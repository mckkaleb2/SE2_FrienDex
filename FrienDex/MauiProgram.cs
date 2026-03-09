using FrienDex.Data.ViewModels;
using FrienDex.Services;
using FrienDex.Views;
using Microsoft.Extensions.Logging;

namespace FrienDex
{
    public class MauiProgram
    {
        //public string logPrefix = "\n\n---------------- MauiProgram.cs --- log ---------\n\n";
        //public string logSuffix = "\n\n----- END ------ MauiProgram.cs --- log ---------\n\n";
        //public string linePrefix = "\n\t>\t-- ";



        //TODO: is it possible to make this method async? I want to seed the database before the app starts, but I don't know if it's possible to do that in an async way. If not, I can just call the SeedDataAsync method synchronously, but it would be nice to have it be async if possible.
        public static MauiApp CreateMauiApp()
        {
            string linePrefix = "\n\t>\t-- ";

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddDbContext<Data.DexContext>();

            #region ServiceRegistration_Pages
            builder.Services.AddSingleton<IPersonRepo, PersonRepo>();


#if DEBUG
            System.Diagnostics.Debug.Write($"{linePrefix}Adding VMs and Pages to Services\n");
#endif
            builder.Services.AddTransient<CreatePersonVM>();
            builder.Services.AddTransient<CreatePersonPage>();
#if DEBUG
            System.Diagnostics.Debug.Write($"{linePrefix}Done! Adding VMs and Pages to Services\n");
#endif


            //builder.Services.AddTransient<TodoItemPage>();
            #endregion ServiceRegistration_Pages
#if DEBUG
            builder.Logging.AddDebug();
#endif
            using (var scope = builder.Services.BuildServiceProvider().CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Data.DexContext>();
#if DEBUG
                db.Database.EnsureDeleted();
#endif
                db.Database.EnsureCreated();
#if DEBUG

                ////var services = builder.Services.BuildServiceProvider();
                //var services = builder.Services.BuildServiceProvider();

                //SeedDataSyncronous(services);

#endif

            }


                return builder.Build();
        } //END MakeMauiApp METHOD


    } // END CLASS
}
