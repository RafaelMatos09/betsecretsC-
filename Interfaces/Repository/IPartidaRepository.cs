using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IPartidaRepository
    {
        Task<PartidaModel> CadastrarPartida(PartidaModel req);
        Task<PartidaModel?> ConsultaPartida(long id);
        Task<List<PartidaModel>> ListaPartidasCampeonato(long campeonatoId, long? rodadaId);
        Task RegistraResultado(long id, int golsCasa, int golsVisitante);
        Task ReagendaPartida(long id, DateTime dataHora);
        Task ExcluiPartida(long id);
    }
}
