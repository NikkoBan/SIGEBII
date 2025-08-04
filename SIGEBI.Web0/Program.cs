using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;
using SIGEBI.IOC.Dependencias;
using SIGEBI.Web0.Extensions;


var builder = WebApplication.CreateBuilder(args);

var apiBaseUrls = builder.Configuration.GetSection("ApiBaseUrls").Get<ApiBaseUrlsConfiguration>()
    ?? throw new InvalidOperationException("La URL base de la API no está configurada.");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SIGEBIContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddApplicationServices(apiBaseUrls.BaseApi);

builder.Services.RegisterSIGEBIDependencies();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
