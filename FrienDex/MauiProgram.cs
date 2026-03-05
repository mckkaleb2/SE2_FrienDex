<<<<<<< Updated upstream
﻿using FrienDex.Data.ViewModels;
using FrienDex.Services;
using FrienDex.Views;
=======
﻿using FrienDex.Services;
>>>>>>> Stashed changes
using Microsoft.Extensions.Logging;

namespace FrienDex
{
    public class MauiProgram
    {
        //TODO: is it possible to make this method async? I want to seed the database before the app starts, but I don't know if it's possible to do that in an async way. If not, I can just call the SeedDataAsync method synchronously, but it would be nice to have it be async if possible.
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
            builder.Services.AddDbContext<Data.DexContext>();

            #region ServiceRegistration_Pages
            builder.Services.AddSingleton<IPersonRepo, PersonRepo>();
            builder.Services.AddSingleton<IInitializerService, InitializerService>();


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

                //var services = builder.Services.BuildServiceProvider();
                var services = builder.Services.BuildServiceProvider();

                SeedDataSyncronous(services);

#endif

            }

<<<<<<< Updated upstream
            builder.Services.AddSingleton<IPersonRepo, PersonRepo>();
            builder.Services.AddTransient<CreatePersonVM>();
            builder.Services.AddTransient<CreatePersonPage>();
=======

#if DEBUG

#endif

>>>>>>> Stashed changes
            return builder.Build();
        } //END MakeMauiApp METHOD

        static async Task SeedDataAsync(IServiceProvider services)
        {
            try
            {

                var db = services.GetRequiredService<Data.DexContext>();
                var personRepo = services.GetRequiredService<Services.IPersonRepo>();
                var initializer = services.GetRequiredService<Services.InitializerService>();

                await initializer.SeedDataAsync();

                //if (!db.Rooms.Any())
                //{
                //    var room1 = new Data.Entities.Room
                //    {
                //        Name = "Living Room",
                //        Description = "A cozy living room with a fireplace."
                //    };
                //    var room2 = new Data.Entities.Room
                //    {
                //        Name = "Kitchen",
                //        Description = "A modern kitchen with stainless steel appliances."
                //    };
                //    db.Rooms.AddRange(room1, room2);
                //await db.SaveChangesAsync();
                //}
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<MauiProgram>>();
                logger.LogError(
                     $"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n"
                    + $"An error occurred while seeding the database. {ex.Message}"
                    //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                    //    Console.WriteLine(
                    + $"\n\n----- 1 -------- SEED DATA ASYNC --- console ----\n\n"
                    //        + $"An error occurred while seeding the database. {ex.Message}"
                    //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                    + $"{ex}"
                    + $"\n\n---- end ------- SEED DATA ASYNC --- console ----\n\n"
                    );
            }
        } // END SeedDataAsync METHOD

        static void SeedDataSyncronous(IServiceProvider services) // or maybe IServiceProvider ?
        {
            try
            {
                var db = services.GetRequiredService<Data.DexContext>();
                var personRepo = services.GetRequiredService<Services.IPersonRepo>();
                var initializer = services.GetRequiredService<Services.InitializerService>();

                SeedDataSyncronous(services);

                //if (!db.Rooms.Any())
                //{
                //    var room1 = new Data.Entities.Room
                //    {
                //        Name = "Living Room",
                //        Description = "A cozy living room with a fireplace."
                //    };
                //    var room2 = new Data.Entities.Room
                //    {
                //        Name = "Kitchen",
                //        Description = "A modern kitchen with stainless steel appliances."
                //    };
                //    db.Rooms.AddRange(room1, room2);
                //    await db.SaveChangesAsync();
                //}


            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<MauiProgram>>();
                logger.LogError(
                     $"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n"
                    + $"An error occurred while seeding the database. {ex.Message}"
                    //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                    //    Console.WriteLine(
                    + $"\n\n----- 1 -------- SEED DATA ASYNC --- console ----\n\n"
                    //        + $"An error occurred while seeding the database. {ex.Message}"
                    //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                    + $"{ex}"
                    + $"\n\n---- end ------- SEED DATA ASYNC --- console ----\n\n"
                    );
            }
        } // END SeedDataAsync METHOD

    } // END CLASS
}
