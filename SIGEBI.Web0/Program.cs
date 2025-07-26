using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Services;
using SIGEBI.IOC.Dependencias;

namespace SIGEBI.Web0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registrar HttpClient para hacer solicitudes a la API
            builder.Services.AddHttpClient();

            // Configura la cadena de conexión para SIGEBIContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Registrar el contexto de la base de datos (SIGEBIContext)
            builder.Services.AddDbContext<SIGEBIContext>(options =>
                options.UseSqlServer(connectionString));

            // Registrar las dependencias necesarias (como IAuthorService)
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IBookService, BookService>();

            // Registrar tus otras dependencias usando tu método de extensión
            builder.Services.RegisterSIGEBIDependencies();

            // Registrar controladores (para MVC o API)
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configurar la tubería de solicitudes HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Configurar las rutas para controladores
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
