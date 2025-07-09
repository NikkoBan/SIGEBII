using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.IOC;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<SIGEBIContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SIGEBIConnection")));


builder.Services.AddProjectDependencies();


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

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddDbContext<SIGEBIContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SIGEBIConnection")));

////builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

//////Los repositorios
////builder.Services.AddScoped<IReservationStatusesRepository, ReservationStatusesRepository>();
////builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
////builder.Services.AddScoped<IReservationHistoryRepository, ReservationHistoryRepository>();

//////Servicios

////builder.Services.AddTransient<IReservationService, ReservationService>();
////builder.Services.AddTransient<IReservationStatusService, ReservationStatusService>();
////builder.Services.AddTransient<IReservationHistoryService, ReservationHistoryService>();

////builder.Services.AddScoped<ReservationApiService>();
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
