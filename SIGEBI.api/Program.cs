using Microsoft.EntityFrameworkCore;
using SIGEBI.IOC.Dependencias;
using SIGEBI.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Obtén la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra el DbContext UNA sola vez
builder.Services.AddDbContext<SIGEBIContext>(options =>
    options.UseSqlServer(connectionString));

// Luego registra las dependencias
builder.Services.RegisterSIGEBIDependencies();

// Registra AutoMapper y otros servicios
builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });



// Configura Swagger (si es necesario)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SIGEBI API", Version = "v1" });
});

var app = builder.Build();

// Configura el middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIGEBI API V1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
