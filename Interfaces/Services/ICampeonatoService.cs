using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface ICampeonatoService
    {
        Task<CampeonatoModel> CadastroCampeonato(CampeonatoModel req);
    }
}
