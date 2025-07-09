using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Validations;
using FluentValidation;
using SIGEBI.Application.DTOs.PublishersDTOs;

namespace SIGEBI.IOC.Dependencies
{
    public static class PublishersDependency
    {
        public static IServiceCollection AddPublishersDependencies(this IServiceCollection services)
        {
            // Repositorio
            services.AddScoped<IPublishersRepository, PublishersRepository>();

            // Servicio de aplicación
            services.AddScoped<IPublishersService, PublishersService>();

            // Validadores FluentValidation
            services.AddScoped<IValidator<CreationPublisherDto>>(sp => PublishersValidation.CreationValidator);
            services.AddScoped<IValidator<UpdatePublisherDto>>(sp => PublishersValidation.UpdateValidator);
            services.AddScoped<IValidator<RemovePublisherDto>>(sp => PublishersValidation.RemoveValidator);

            return services;
        }
    }
}
