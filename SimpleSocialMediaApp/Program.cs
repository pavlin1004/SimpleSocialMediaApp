using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleSocialApp.Data;
using SimpleSociaMedialApp;

var builder = WebApplication.CreateBuilder(args);

// Configure configuration
builder.Configuration.AddAppConfiguration(builder.Environment);

// Register services
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddCloudinarySupport(builder.Configuration);
builder.Services.AddAppServices();
builder.Services.AddIdentityAndAuth();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Optional: keep if dev only
builder.Services.AddSignalR();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<ChatHub>("/chathub");

// Seed dev data
await app.SeedDevelopmentDataAsync();

app.Run();
