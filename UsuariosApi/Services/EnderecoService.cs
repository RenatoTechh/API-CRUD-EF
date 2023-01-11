using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class EnderecoService
    {
        public async Task<Usuario> PesquisaEnderecoPorCep(string cep)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            var content = await response.Content.ReadAsStringAsync();

            var endereco = JsonConvert.DeserializeObject<Usuario>(content);

            return endereco;
        }
    }
}
