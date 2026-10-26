using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;

var builder = WebApplication.CreateBuilder(args);

// Servicios: ASP.NET Core MVC (controladores + vistas), sin Razor Pages.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TorneoEsportsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
   .WithStaticAssets();

app.Run();
