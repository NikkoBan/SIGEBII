using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Web0.Interfaces.Book;
using SIGEBI.Web0.Interfaz.Author;
using SIGEBI.Web0.Interfaz.BookAuthor;
using SIGEBI.Web0.Repositories.AuthorRepository;
using SIGEBI.Web0.Repositories.BookRepository;
using SIGEBI.Web0.Repositories.BookAuthorRepository;

namespace SIGEBI.Web0.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, string baseApiUrl)
        {
            services.AddHttpClient<IAuthorWeb, AuthorWebRepository>(client =>
                client.BaseAddress = new Uri(baseApiUrl));

            services.AddHttpClient<IBookWeb, BookWebRepository>(client =>
                client.BaseAddress = new Uri(baseApiUrl));

            services.AddHttpClient<IBookAuthorWeb, BookAuthorWebRepository>(client =>
                client.BaseAddress = new Uri(baseApiUrl));



            return services;
        }
    }
}
