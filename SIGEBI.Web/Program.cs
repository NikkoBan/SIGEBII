// SIGEBI.Web/Program.cs

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; // Necesario para IConfiguration

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // Habilita el patrón MVC

// --- Configuración CRÍTICA para consumo de API (basado en el video) ---
builder.Services.AddHttpClient(); // Registra HttpClientFactory para inyectar HttpClient

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Para ver errores detallados en desarrollo
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Página de error personalizada para producción
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirige las solicitudes HTTP a HTTPS
app.UseStaticFiles();      // Permite servir archivos estáticos (CSS, JS, imágenes) desde wwwroot

app.UseRouting();

app.UseAuthentication(); // Si usas autenticación (ej. cookies de ASP.NET Core Identity)
app.UseAuthorization();  // Habilita el middleware de autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Establece Home/Index como la ruta por defecto

app.Run();