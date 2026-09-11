using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IBairroRepository
    {
        Task<BairroModel> CadastraBairro(BairroModel req);
        Task<List<BairroModel>> ListaBairros();
        Task<BairroModel?> ConsultaBairro(int id);
        Task<List<BairroModel>> ListaBairrosPorCidade(string cidade);
        Task AtualizaBairro(BairroModel req);
        Task ExcluiBairro(int id);
    }
}
