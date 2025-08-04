using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interface;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Configuración Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de DbContext con cadena de conexión
builder.Services.AddDbContext<SIGEBIDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Registro de repositorios
builder.Services.AddScoped<ILoanHistoryRepository, LoanHistoryRepository>();
builder.Services.AddScoped<ILoanStatusRepository, LoanStatusRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ILoanDetailsRepository, LoanDetailsRepository>();

// Registro de servicios
builder.Services.AddScoped<ILoanHistoryService, LoanHistoryService>();
builder.Services.AddScoped<ILoanStatusService, LoanStatusService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<ILoanDetailsService, LoanDetailsService>();

// Configuración de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configuración del pipeline HTTP
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

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // Si usas autenticación, de lo contrario puedes quitarlo
app.UseAuthorization();

app.MapControllers();

app.Run();
