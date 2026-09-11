using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface ICampeonatoTimeService
    {
        Task<CampeonatoTimeModel> CadastrarCampeonatoTime(CampeonatoTimeModel req);
        Task<List<CampeonatoTimeModel>> ListaTimesCampeonato(long campeonatoId); 
        Task AtualizaGrupo(CampeonatoTimeModel req); 
        Task ExcluiCampeonatoTime(long campeonatoId, long timeId);
    }
}
