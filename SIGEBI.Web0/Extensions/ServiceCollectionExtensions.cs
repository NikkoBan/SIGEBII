using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Services;
using SIGEBI.Web0.Interfaces.Book;
using SIGEBI.Web0.Repositories.BookRepository;
using SIGEBI.Web0.Repositories.AuthorRepository;
using SIGEBI.Web0.Services.Book;
using SIGEBI.Web0.Interfaz.BookAuthor;
using SIGEBI.Web0.Repositories.BookAuthorRepository;
using SIGEBI.Web0.Services.BookAuthor;
using SIGEBI.Web0.Interfaz.Author;
using SIGEBI.Web0.Services.Author; 

namespace SIGEBI.Web0.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string apiBaseUrl)
        {
            
            services.AddScoped<IBookWebService, BookWebService>();
            services.AddHttpClient<IBookWeb, BookWebRepository>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            services.AddScoped<IAuthorWebService, AuthorWebService>();
            services.AddHttpClient<IAuthorWeb, AuthorWebRepository>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            services.AddScoped<IBookAuthorWebService, BookAuthorWebService>();
            services.AddHttpClient<IBookAuthorWeb, BookAuthorWebRepository>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IAuthorBookService, BookAuthorService>();

            return services;
        }
    }
}
