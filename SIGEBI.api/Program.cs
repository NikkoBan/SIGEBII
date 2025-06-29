using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SIGEBIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Publishers Repository Registration
builder.Services.AddScoped<IPublishersRepository, PublishersRepository>();

// Publishers Service Registration
builder.Services.AddScoped<IPublishersService, PublishersService>(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
