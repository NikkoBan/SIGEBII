using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SIGEBI.Application.mappers;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Persistence.Context;

using SIGEBI.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. OBTENER LA CADENA DE CONEXIÓN ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// --- 2. REGISTRAR AUTOMAPPER ---
builder.Services.AddAutoMapper(typeof(AuthorProfile).Assembly);

// --- 3. REGISTRAR EL DBContext PARA EF CORE ---
builder.Services.AddDbContext<SIGEBIContext>(options =>
    options.UseSqlServer(connectionString)
);

// --- 4. REGISTRAR LOS REPOSITORIOS ---
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<AuthorRepository>>();
    return new AuthorRepository(connectionString, logger);
});

builder.Services.AddScoped<IBookAuthorRepository, BookAuthorRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<BookAuthorRepository>>();
    return new BookAuthorRepository(connectionString, logger);
});

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

// --- 5. REGISTRAR LOS SERVICIOS ---
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookAuthorService, BookAuthorService>();

// --- 6. CONFIGURACIÓN ESTÁNDAR DE ASP.NET CORE ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization(); // Asegúrate de tener autenticación/autorización configurada si la usas.

app.MapControllers();

app.Run();
