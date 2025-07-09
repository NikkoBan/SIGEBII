using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositori;
// using SIGEBI.Persistence.Base;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Application.Profiles;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SIGEBIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// BaseRepository ES ABSTRACTO
// builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserHistoryService, UserHistoryService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();