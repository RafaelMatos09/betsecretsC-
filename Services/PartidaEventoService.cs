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
    }
}
