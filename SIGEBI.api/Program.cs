// SIGEBI.api/Program.cs

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration; // Necesario para IConfiguration

// Usings para Entity Framework Core y tu DbContext
using Microsoft.EntityFrameworkCore; // Para el m�todo UseSqlServer
using SIGEBI.Persistence.Context;    // Para tu clase DbContext (ej. SIGEBIDbContext)

// --- �AQU� EST� LA L�NEA CR�TICA QUE FALTABA! ---
using SIGEBI.Persistence.Base; // <-- NECESARIO para OperationResult, sus m�todos IsSuccess, Success, Failure.
// --- FIN DE L�NEA CR�TICA ---

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

// Configuraci�n de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- REGISTRO DE SERVICIOS PERSONALIZADOS (Capas de tu Soluci�n) ---

// CONFIGURACI�N CR�TICA DEL DB CONTEXT Y LA CONEXI�N A LA BASE DE DATOS
builder.Services.AddDbContext<SIGEBIDbContext>(options =>
{
    // Lee la cadena de conexi�n "DefaultConnection" desde appsettings.json
    // y la usa para configurar SQL Server.
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// REGISTRO DE REPOSITORIOS
// Aseg�rate de que las implementaciones concretas de tus repositorios est�n en SIGEBI.Persistence.Repositori
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();
// Si tienes otros repositorios, reg�stralos aqu� (ej. IBookRepository, ICategoryRepository)

// REGISTRO DE SERVICIOS DE LA CAPA DE APLICACI�N
// Aseg�rate de que las implementaciones de tus servicios est�n en SIGEBI.Application.Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserHistoryService, UserHistoryService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>(); // UserAccountService debe implementar IUserAccountService

// Configuraci�n de AutoMapper
// Aseg�rate de que tu MappingProfile est� definido en el ensamblado de SIGEBI.Application
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Configuraci�n de ILogger (para ver los logs en la consola)
builder.Logging.ClearProviders(); // Opcional: limpia los proveedores de logging predeterminados
builder.Logging.AddConsole();    // Agrega el proveedor de consola
builder.Logging.AddDebug();      // Agrega el proveedor de depuraci�n (salida de depuraci�n de VS)


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

app.UseAuthentication(); // Si usas autenticaci�n (ej. JWT)
app.UseAuthorization();  // Habilita el middleware de autorizaci�n

app.MapControllers(); // Mapea los atributos de controlador a endpoints

app.Run();