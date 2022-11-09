using JSarad_C868_Capstone.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;  // cookie new

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//inject connection

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
{
    options.Cookie.Name = "CookieAuth";
    options.LoginPath = "/Login/Index";
});
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//            .AddCookie(options =>
//            {
//                options.LoginPath = "/Home/Login";
//                //options.Cookie.Name = ".AspNetCore.Cookies";
//                //options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
//                //options.SlidingExpiration = true;  //don't know what all this is yet

//            }); //new


var app = builder.Build();

//automatically run initializer on app startup
//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;
//var initializer = services.GetRequiredService<DbInitializer>();
//initializer.Run();



//original automatic migration
using (var scope = app.Services.CreateScope())
using (
    var dbContext = scope.ServiceProvider.GetService<AppDbContext>()) dbContext.Database.Migrate();

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

app.UseAuthentication(); //cookie new
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");  //cookie change
//pattern: "{controller=Event}/{action=Index}/{id?}");  //cookie new

app.Run();
