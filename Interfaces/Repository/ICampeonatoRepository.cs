using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ICampeonatoRepository
    {
        Task<CampeonatoModel> CadastroCampeonato(CampeonatoModel req);
    }
}
