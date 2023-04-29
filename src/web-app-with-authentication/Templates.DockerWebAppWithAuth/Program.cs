using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Templates.DockerWebAppWithAuth;
using Templates.DockerWebAppWithAuth.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container:
var appSettingsEnvironment = builder.Configuration.GetValue<string>("AppSettingsEnvironment");

builder.Services.AddDbContext<WebAppDbContext>(options =>
{
    switch (appSettingsEnvironment)
    {
        case Constants.AppSettingsLocalEnvironment: // For testing purposes only and fast startup time for demos
            options.UseInMemoryDatabase(databaseName: "WebAppWithAuthentication");
            break;
        case Constants.AppSettingsDevelopmentEnvironment: // If you are going this route, make sure you have MsSQLLocalDB installed
            var sqlLocalDbConnectionString = builder.Configuration.GetConnectionString("SqlLocalDbConnection");
            options.UseSqlServer(sqlLocalDbConnectionString);
            break;
        default:
            throw new NotImplementedException("Environment not handled in the code.");
    }
});

if (Constants.DevelopmentEnvironments.Contains(appSettingsEnvironment))
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

// You can use your own class here to customize the data you can store regarding users
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    if (!Constants.DevelopmentEnvironments.Contains(appSettingsEnvironment))
    {
        options.SignIn.RequireConfirmedAccount = true;
    }
}).AddEntityFrameworkStores<WebAppDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
