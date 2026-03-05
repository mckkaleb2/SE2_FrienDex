using FrienDex.Data.ViewModels;
using FrienDex.Services;
using FrienDex.Views;
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
            builder.Services.AddDbContext<Data.DexContext>();
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
            }

            builder.Services.AddSingleton<IPersonRepo, PersonRepo>();
            builder.Services.AddTransient<CreatePersonVM>();
            builder.Services.AddTransient<CreatePersonPage>();
            return builder.Build();
        }
    }
}
