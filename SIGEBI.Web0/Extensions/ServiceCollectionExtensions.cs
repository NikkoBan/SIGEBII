using SIGEBI.Web0.Interfaces.Book;
using SIGEBI.Web0.Interfaz.Author;
using SIGEBI.Web0.Interfaz.BookAuthor;
using SIGEBI.Web0.Repositories.AuthorRepository;
using SIGEBI.Web0.Repositories.BookRepository;
using SIGEBI.Web0.Repositories.BookAuthorRepository;
using SIGEBI.Web0.Services.Author;
using SIGEBI.Web0.Services.Book;
using SIGEBI.Web0.Services.BookAuthor;

namespace SIGEBI.Web0.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string apiBaseUrl)
        {
           
            services.AddHttpClient<IAuthorWeb, AuthorWebRepository>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            services.AddHttpClient<IBookWeb, BookWebRepository>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            services.AddHttpClient<IBookAuthorWeb, BookAuthorWebRepository>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            
            services.AddScoped<IAuthorWebService, AuthorWebService>();
            services.AddScoped<IBookWebService, BookWebService>();
            services.AddScoped<IBookAuthorWebService, BookAuthorWebService>();

            return services;
        }
    }
}
