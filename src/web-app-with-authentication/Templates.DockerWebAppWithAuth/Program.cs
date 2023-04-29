using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Templates.DockerWebAppWithAuth;
using Templates.DockerWebAppWithAuth.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container:
var appSettingsEnvironment = builder.Configuration.GetValue<string>("AppSettingsEnvironment");

// Entity Framework:
builder.Services.AddDbContext<WebAppDbContext>(options =>
{
    switch (appSettingsEnvironment)
    {
        case Constants.AppSettingsLocalEnvironment:
            // For testing purposes only and fast startup time for demos, does not need migrations
            options.UseInMemoryDatabase(databaseName: "WebAppWithAuthentication");
            break;
        case Constants.AppSettingsDevEnvironment:
            // If you are going this route, make sure you have MSSQLLocalDB installed and the default connectionString included in appsettings.json
            // should work, you can also install or add your preferred provider to connect to your DB of need (npgsql, etc.)
            // Make sure you generate initial Migration, script included at .csproj root (TODO LF)
            var sqlLocalDbConnectionString = builder.Configuration.GetConnectionString("SqlLocalDbConnection");
            options.UseSqlServer(sqlLocalDbConnectionString);
            break;
        case Constants.AppSettingsReleaseEnvironment:
            // For testing purposes only, you should use your Productive DB here:
            options.UseInMemoryDatabase(databaseName: "WebAppWithAuthenticationRelease");
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
