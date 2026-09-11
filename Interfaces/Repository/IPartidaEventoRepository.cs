using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IPartidaEventoRepository
    {
        Task<PartidaEventoModel> CadastrarPartidaEvento(PartidaEventoModel req);
        Task<List<PartidaEventoModel>> ListaEventosPartida(long partidaId);
        Task<List<PartidaEventoModel>> ListaArtilharia(long campeonatoId);
        Task AtualizaPartidaEvento(PartidaEventoModel req);
        Task ExcluiPartidaEvento(long id);
    }
}
