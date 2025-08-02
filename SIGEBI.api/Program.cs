using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Agregar el contexto de base de datos
builder.Services.AddDbContext<SIGEBIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias de repositorios
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ILoanHistoryRepository, LoanHistoryRepository>();
// Puedes seguir agregando más repositorios aquí...

// Agregar controladores con opciones de serialización para evitar ciclos infinitos
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Agregar documentación Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de entorno
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
