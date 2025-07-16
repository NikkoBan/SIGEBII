using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Services;
using Microsoft.Extensions.Logging;

namespace SIGEBI.IOC.Dependencias
{
    public static class BookDependecy
    {
        public static void Register(IServiceCollection services)
        {
            // Repositorio simple (sin factory)
            services.AddScoped<IBookRepository, BookRepository>();

            // Servicio
            services.AddScoped<IBookService, BookService>();
        }
    }
}
