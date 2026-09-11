using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class PartidaService : IPartidaService
    {
        private readonly IPartidaRepository _partidaRepository;

        public PartidaService(IPartidaRepository partidaRepository)
        {
            _partidaRepository = partidaRepository;
        }

        public async Task<PartidaModel> CadastrarPartida(PartidaModel req)
        {
            return await _partidaRepository.CadastrarPartida(req);
        }
    }
}
