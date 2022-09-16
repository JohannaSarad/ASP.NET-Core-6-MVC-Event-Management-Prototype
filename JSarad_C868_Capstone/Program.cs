using JSarad_C868_Capstone.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//inject connection
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
//adding new initializer to dependency injection
//builder.Services.AddTransient<DbInitializer>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
