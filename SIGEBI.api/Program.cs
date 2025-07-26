// SIGEBI.api/Program.cs

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration; // Necesario para IConfiguration

// Usings para Entity Framework Core y tu DbContext
using Microsoft.EntityFrameworkCore; // Para el método UseSqlServer
using SIGEBI.Persistence.Context;    // Para tu clase DbContext (ej. SIGEBIDbContext)

// --- ¡AQUÍ ESTÁ LA LÍNEA CRÍTICA QUE FALTABA! ---
using SIGEBI.Persistence.Base; // <-- NECESARIO para OperationResult, sus métodos IsSuccess, Success, Failure.
// --- FIN DE LÍNEA CRÍTICA ---

// Usings para tus propias capas y servicios
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services; // Para las implementaciones de los servicios
using SIGEBI.Persistence.Interfaces; // Para las interfaces de repositorios
using SIGEBI.Persistence.Repositori; // Para las implementaciones de repositorios y UserAccountService
using AutoMapper; // Para AutoMapper
using SIGEBI.Application.Profiles; // Para el MappingProfile

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- REGISTRO DE SERVICIOS PERSONALIZADOS (Capas de tu Solución) ---

// CONFIGURACIÓN CRÍTICA DEL DB CONTEXT Y LA CONEXIÓN A LA BASE DE DATOS
builder.Services.AddDbContext<SIGEBIDbContext>(options =>
{
    // Lee la cadena de conexión "DefaultConnection" desde appsettings.json
    // y la usa para configurar SQL Server.
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// REGISTRO DE REPOSITORIOS
// Asegúrate de que las implementaciones concretas de tus repositorios estén en SIGEBI.Persistence.Repositori
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();
// Si tienes otros repositorios, regístralos aquí (ej. IBookRepository, ICategoryRepository)

// REGISTRO DE SERVICIOS DE LA CAPA DE APLICACIÓN
// Asegúrate de que las implementaciones de tus servicios estén en SIGEBI.Application.Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserHistoryService, UserHistoryService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>(); // UserAccountService debe implementar IUserAccountService

// Configuración de AutoMapper
// Asegúrate de que tu MappingProfile esté definido en el ensamblado de SIGEBI.Application
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Configuración de ILogger (para ver los logs en la consola)
builder.Logging.ClearProviders(); // Opcional: limpia los proveedores de logging predeterminados
builder.Logging.AddConsole();    // Agrega el proveedor de consola
builder.Logging.AddDebug();      // Agrega el proveedor de depuración (salida de depuración de VS)


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirige las solicitudes HTTP a HTTPS
app.UseRouting(); // Habilita el enrutamiento

app.UseAuthentication(); // Si usas autenticación (ej. JWT)
app.UseAuthorization();  // Habilita el middleware de autorización

app.MapControllers(); // Mapea los atributos de controlador a endpoints

app.Run();