using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AmazonWebAppMVC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddDbContext<AmazonWebAppMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AmazonWebAppMVCContext") ?? throw new InvalidOperationException("Connection string 'AmazonWebAppMVCContext' not found.")));


// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AspNetCore.Cookies";
        options.LoginPath = "/Auth/Login"; // Customize the login path
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Customize the access denied path
    });

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Client", policy => policy.RequireRole("Client"));
});

//add Serilog
builder.Logging.AddSerilog(new LoggerConfiguration()
       .WriteTo.File("..\\Logs\\amazonwebappmvc.log", rollingInterval: RollingInterval.Day)
       .CreateLogger());
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Order.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.MaxAge = TimeSpan.FromMinutes(30);

});

// build 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Logger.LogInformation("Application AmazonWebAppMVC started");

app.Run();
