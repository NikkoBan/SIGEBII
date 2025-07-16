using Microsoft.Extensions.DependencyInjection;



namespace SIGEBI.IOC.Dependencias
{
    public static class DependencyInjection
    {
       
           public static IServiceCollection RegisterSIGEBIDependencies(this IServiceCollection services)
           {
            AuthorDependecy.Register(services);
            AuthorsBookDependency.Register(services);
            BookDependecy.Register(services);
          
            return services;
           }
    }
}
