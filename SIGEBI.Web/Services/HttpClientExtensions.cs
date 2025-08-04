using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SIGEBI.Web.Services
{
    public static class HttpClientExtensions
    {
        public static async Task<T?> GetOrDefaultAsync<T>(this HttpClient client, string url)
        {
            try { return await client.GetFromJsonAsync<T>(url); }
            catch { return default; }
        }

        public static async Task<bool> PostJsonOkAsync<T>(this HttpClient client, string url, T data)
        {
            try { var r = await client.PostAsJsonAsync(url, data); return r.IsSuccessStatusCode; }
            catch { return false; }
        }

        public static async Task<bool> PutJsonOkAsync<T>(this HttpClient client, string url, T data)
        {
            try { var r = await client.PutAsJsonAsync(url, data); return r.IsSuccessStatusCode; }
            catch { return false; }
        }

        public static async Task<bool> DeleteOkAsync(this HttpClient client, string url)
        {
            try { var r = await client.DeleteAsync(url); return r.IsSuccessStatusCode; }
            catch { return false; }
        }
    }
}