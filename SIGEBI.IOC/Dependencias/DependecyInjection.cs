using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SIGEBI.Persistence.Context;
using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Services;

namespace SIGEBI.IOC.Dependencias
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterSIGEBIDependencies(this IServiceCollection Services)
        {
            Services.AddScoped<IAuthorRepository>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<AuthorRepository>>();
                var context = provider.GetRequiredService<SIGEBIContext>();
                return new AuthorRepository(context, logger);
            });

            Services.AddScoped<IBookRepository>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BookRepository>>();
                var context = provider.GetRequiredService<SIGEBIContext>();
                return new BookRepository(context, logger);
            });

            Services.AddScoped<IBookAuthorRepository>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BookAuthorRepository>>();
                var context = provider.GetRequiredService<SIGEBIContext>();
                return new BookAuthorRepository(context, logger);
            });



            Services.AddScoped<IAuthorService, AuthorService>();
            Services.AddScoped<IBookService, BookService>();
            Services.AddScoped<IAuthorBookService, BookAuthorService>();
          

            return Services;
        }
    }
}
