using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AmazonWebAppMVC.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AmazonWebAppMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AmazonWebAppMVCContext") ?? throw new InvalidOperationException("Connection string 'AmazonWebAppMVCContext' not found.")));

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

app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
