using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class PartidaEventoService : IPartidaEventoService
    {
        private readonly IPartidaEventoRepository _partidaEventoRepository;

        public PartidaEventoService(IPartidaEventoRepository partidaEventoRepository)
        {
            _partidaEventoRepository = partidaEventoRepository;
        }

        public async Task<PartidaEventoModel> CadastrarPartidaEvento(PartidaEventoModel req)
        {
            return await _partidaEventoRepository.CadastrarPartidaEvento(req);
        }
        public Task<List<PartidaEventoModel>> ListaEventosPartida(long partidaId) => _partidaEventoRepository.ListaEventosPartida(partidaId);
        public Task<List<PartidaEventoModel>> ListaArtilharia(long campeonatoId) => _partidaEventoRepository.ListaArtilharia(campeonatoId);
        public Task AtualizaPartidaEvento(PartidaEventoModel req) => _partidaEventoRepository.AtualizaPartidaEvento(req);
        public Task ExcluiPartidaEvento(long id) => _partidaEventoRepository.ExcluiPartidaEvento(id);
    }
}
