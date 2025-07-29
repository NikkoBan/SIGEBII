using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIGEBI.Web.Repositories;
using SIGEBI.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// --- Registro de HttpClient para consumo de API REST ---
builder.Services.AddHttpClient(); // Para inyectar HttpClient manualmente si alguna vez lo necesitas directo

// Registro de repositorios (patrón Repository para consumo de API)
builder.Services.AddHttpClient<IUserApiRepository, UserApiRepository>();
builder.Services.AddHttpClient<IUserHistoryApiRepository, UserHistoryApiRepository>();

// Registro de ApiClients (si prefieres usar clases directas tipo ApiClient)
builder.Services.AddHttpClient<UserApiClient>();
builder.Services.AddHttpClient<UserHistoryApiClient>();

// ... Puedes agregar aquí otros servicios, autenticación, etc.

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