using betsecrets.Modelos;
using System.Text.Json;

namespace betsecrets.Interfaces.Services
{
    public class ApiFutebolService
    {
        private readonly HttpClient _httpClient;

        public ApiFutebolService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       
public async Task<List<TabelaCampeonatoModel>> BuscarTabela()
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.api-futebol.com.br/v1/campeonatos/10/tabela"
            );

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    "test_f326d0e309806e29120709c7e0ceec"
                );

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    $"Erro ao buscar tabela do campeonato 10: {(int)response.StatusCode} - {errorBody}",
                    null,
                    response.StatusCode
                );
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<TabelaCampeonatoModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<TabelaCampeonatoModel>();
        }


    }
}
