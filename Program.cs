using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;

// =========================================================================================
// PUNTO DE ENTRADA Y CONFIGURACIÓN DE LA APLICACIÓN (ASP.NET Core)
// =========================================================================================

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS EN EL CONTENEDOR DE INYECCIÓN DE DEPENDENCIAS (IoC)
// -----------------------------------------------------------------------------
// Habilita el patrón MVC completo (Controladores + Vistas Razor).
builder.Services.AddControllersWithViews();

// Registra el DbContext de Entity Framework Core con la cadena de conexión de appsettings.json.
// Esto permite inyectar 'TorneoEsportsContext' automáticamente en los constructores de los controladores.
builder.Services.AddDbContext<TorneoEsportsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TorneoEsportsContext>();
    db.Database.Migrate();
    DbInitializer.Sembrar(db);
}

// 2. CONFIGURACIÓN DEL PIPELINE DE MIDDLEWARES (FLUJO DE PETICIONES HTTP)
// -----------------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirecciona automáticamente peticiones HTTP no seguras hacia HTTPS.
app.UseHttpsRedirection();

// Habilita el enrutamiento: inspecciona la URL y determina el endpoint correspondiente.
app.UseRouting();

// Middleware de autorización.
app.UseAuthorization();

// Sirve archivos estáticos optimizados (CSS, JS, imágenes de wwwroot).
app.MapStaticAssets();

// RUTA POR DEFECTO DEL SISTEMA:
// Si la URL es "/", se traduce a:
// Controlador = "Home", Acción = "Index", Parámetro opcional = id.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
   .WithStaticAssets();

// Arranca la aplicación web Kestrel en el puerto configurado.
app.Run();
