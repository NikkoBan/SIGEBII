using Microsoft.Extensions.DependencyInjection; 
using SIGEBI.Application.Interfaces; 
using SIGEBI.Application.Services; 
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositori; 
using SIGEBI.Persistence.Base; 
using AutoMapper; 
using SIGEBI.Application.Profiles; 
using SIGEBI.Persistence.Context; 

namespace SIGEBI.IOC.Dependencies 
{
    public static class UserDependencies
    {
        public static void AddUserModuleDependencies(this IServiceCollection services)
        {
            // Registrar los repositorios de persistencia
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

            // Registrar los servicios de aplicación (Business Logic Layer)
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserHistoryService, UserHistoryService>();
            services.AddScoped<IUserAccountService, UserAccountService>();

            // Configurar AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}