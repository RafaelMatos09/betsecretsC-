using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IBairroRepository
    {
        Task<BairroModel> CadastraBairro(BairroModel req);
        Task<List<BairroModel>> ListaBairros();
    }
}
