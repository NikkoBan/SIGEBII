using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIGEBI.Web.Repositories;
using SIGEBI.Web.Repositories.interfaces;
using SIGEBI.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// --- Registro de HttpClient para consumo de API REST ---
builder.Services.AddHttpClient(); // Para inyectar HttpClient manualmente si alguna vez lo necesitas directo

// Registro de repositorios usando las NUEVAS interfaces y clases desacopladas
builder.Services.AddHttpClient<IUserWebRepository, UserWebRepository>();
builder.Services.AddHttpClient<IUserHistoryWebRepository, UserHistoryWebRepository>();

// ... Agrega aquí otros servicios, autenticación, etc.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();