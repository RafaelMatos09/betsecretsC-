using betsecrets.Modelos;
using betsecrets.Modelos.Request;

namespace betsecrets.Interfaces.Services
{
    public interface IBairroService
    {
        Task<BairroModel> CadastraBairro(BairroModel req);
        Task<List<BairroModel>> ListaBairros();
        Task<BairroModel?> ConsultaBairro(int id); 
        Task<List<BairroModel>> ListaBairrosPorCidade(string cidade); 
        Task AtualizaBairro(BairroModel req); 
        Task ExcluiBairro(int id);
    }
}
