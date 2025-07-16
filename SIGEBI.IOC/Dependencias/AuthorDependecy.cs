using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Services;
using Microsoft.Extensions.Logging;

namespace SIGEBI.IOC.Dependencias
{
    public static class AuthorDependecy
    {
             public static void Register(IServiceCollection services)
             {
            services.AddScoped<IAuthorRepository>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<AuthorRepository>>();
                var context = provider.GetRequiredService<SIGEBIContext>();
                return new AuthorRepository(context, logger);
            });


            services.AddScoped<IAuthorService, AuthorService>();
        }
    }
}
