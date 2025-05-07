using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Data.Seeders;
using SimpleSociaMedialApp.Services.External.Implementations;
using SimpleSociaMedialApp.Services.External.Interfaces;
using SimpleSociaMedialApp.Services.Functional.Implementations;
using SimpleSociaMedialApp.Services.Functional.Interfaces;
using SimpleSociaMedialApp.Services.Utilities.Implementations;
using SimpleSociaMedialApp.Services.Utilities.Interfaces;

namespace SimpleSociaMedialApp
{
    public static class AppExtensions
    {
        public static IConfigurationBuilder AddAppConfiguration(this IConfigurationBuilder config, IWebHostEnvironment env)
        {
            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                config.AddUserSecrets<Program>();
            }

            return config;
        }
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");

            services.AddDbContext<SocialDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                       .UseSqlServer(connectionString, sql => sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            return services;
        }

        public static IServiceCollection AddCloudinarySupport(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("Cloudinary");
            var cloudinary = new Cloudinary(new Account(
                section["CloudName"],
                section["ApiKey"],
                section["ApiSecret"]
            ));

            services.AddSingleton(cloudinary);
            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IFriendshipService, FriendshipsService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<ISeeder, Seeder>();
            services.AddScoped<IFakePersonService, FakePersonService>();
            services.AddTransient<IChatService, ChatService>();
            return services;
        }

        public static IServiceCollection AddIdentityAndAuth(this IServiceCollection services)
        {
            services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<SocialDbContext>();

            services.AddAuthentication()
                .AddCookie(opt => opt.LoginPath = "/Account/Login");

            return services;
        }

        public static async Task SeedDevelopmentDataAsync(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment()) return;

            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
            await seeder.SeedAsync();
        }
    }
}
