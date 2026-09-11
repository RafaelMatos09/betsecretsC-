using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ICampeonatoRepository
    {
        Task<CampeonatoModel> CadastroCampeonato(CampeonatoModel req);
        Task<CampeonatoModel?> ConsultaCampeonato(long id);
        Task<List<CampeonatoModel>> ListaCampeonatos();
        Task AtualizaCampeonato(CampeonatoModel req);
        Task ExcluiCampeonato(long id);
    }
}
