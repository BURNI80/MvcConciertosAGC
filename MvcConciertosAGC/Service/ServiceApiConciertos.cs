using MvcConciertosAGC.Helpers;
using MvcConciertosAGC.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcConciertosAGC.Service
{
    public class ServiceApiConciertos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServiceApiConciertos(IConfiguration configuration)
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");

            SecretAWS secretos = HelperSecret.GetSecret().Result;
            this.UrlApi = secretos.API;
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task CreateEvento(Evento evento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/CreateEvento";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string jsonComic = JsonConvert.SerializeObject(evento);
                StringContent content = new StringContent(jsonComic, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(this.UrlApi + request, content);
            }
        }

        public async Task<List<Categoria>> GetCategorias()
        {
            string request = "api/GetCategorias";
            List<Categoria> categorias = await this.CallApiAsync<List<Categoria>>(request);
            return categorias;
        }

        public async Task<List<Evento>> GetEventos()
        {
            string request = "api/GetEventos";
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }

        public async Task<List<Evento>> GetEventosCategoria(int id)
        {
            string request = "api/GetEventosCategoria/"+id;
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        } 


    }
}
