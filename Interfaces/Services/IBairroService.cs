using betsecrets.Modelos;
using betsecrets.Modelos.Request;

namespace betsecrets.Interfaces.Services
{
    public interface IBairroService
    {
        Task<BairroModel> CadastraBairro(BairroModel req);
    }
}
