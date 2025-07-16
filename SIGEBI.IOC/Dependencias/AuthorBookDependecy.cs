
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Services;
using Microsoft.Extensions.Logging;

namespace SIGEBI.IOC.Dependencias
{
    public static class AuthorsBookDependency
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IBookAuthorRepository>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BookAuthorRepository>>();
                var context = provider.GetRequiredService<SIGEBIContext>();
                return new BookAuthorRepository(context, logger);
            });

            services.AddScoped<IAuthorBookService, BookAuthorService>();
        }
    }
}
