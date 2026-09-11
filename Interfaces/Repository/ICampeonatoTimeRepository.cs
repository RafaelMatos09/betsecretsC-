using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ICampeonatoTimeRepository
    {
        Task<CampeonatoTimeModel> CadastrarCampeonatoTime(CampeonatoTimeModel req);
        Task<List<CampeonatoTimeModel>> ListaTimesCampeonato(long campeonatoId);
        Task AtualizaGrupo(CampeonatoTimeModel req);
        Task ExcluiCampeonatoTime(long campeonatoId, long timeId);
    }
}
