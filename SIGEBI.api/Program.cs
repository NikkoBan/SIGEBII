using Microsoft.EntityFrameworkCore;

using SIGEBI.Persistence.Context;

using SIGEBI.IOC.Dependencias;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<SIGEBIContext>(options =>
    options.UseSqlServer(connectionString));

// Registro de dependencias por módulos, todo en una línea
builder.Services.RegisterSIGEBIDependencies();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SIGEBI API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIGEBI API V1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();