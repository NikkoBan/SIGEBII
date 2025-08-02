using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.IOC.Dependencias;
using SIGEBI.Web0.Extensions;
using SIGEBI.Web0.Models; // Necesitas un modelo para ApiBaseUrlsConfiguration

var builder = WebApplication.CreateBuilder(args);

// Obtener la configuración de las URLs de la API
var apiBaseUrls = builder.Configuration.GetSection("ApiBaseUrls").Get<ApiBaseUrlsConfiguration>()
    ?? throw new InvalidOperationException("La URL base de la API no está configurada.");

// Configurar la conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SIGEBIContext>(options =>
    options.UseSqlServer(connectionString));

// Aquí llamamos a tu método de extensión y le pasamos la URL base de la API.
// Esto es mucho más limpio que registrar todos los servicios aquí.
builder.Services.AddApplicationServices(apiBaseUrls.BaseApi);

// Registrar dependencias adicionales
builder.Services.RegisterSIGEBIDependencies();

// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configurar las rutas del controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
