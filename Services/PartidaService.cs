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
        public Task<PartidaModel?> ConsultaPartida(long id) => _partidaRepository.ConsultaPartida(id);
        public Task<List<PartidaModel>> ListaPartidasCampeonato(long campeonatoId, long? rodadaId) => _partidaRepository.ListaPartidasCampeonato(campeonatoId, rodadaId);
        public Task RegistraResultado(long id, int golsCasa, int golsVisitante) => _partidaRepository.RegistraResultado(id, golsCasa, golsVisitante);
        public Task ReagendaPartida(long id, DateTime dataHora) => _partidaRepository.ReagendaPartida(id, dataHora);
        public Task ExcluiPartida(long id) => _partidaRepository.ExcluiPartida(id);
    }
}
