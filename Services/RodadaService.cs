using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class RodadaService : IRodadaService
    {
        private readonly IRodadaRepository _rodadaRepository;
        
        public RodadaService(IRodadaRepository rodadaRepository)
        {
            _rodadaRepository = rodadaRepository;
        }

        public async Task<RodadaModel> CadastrarRodada(RodadaModel req)
        {
            return await _rodadaRepository.CadastrarRodada(req);
        }
        public Task<List<RodadaModel>> ListaRodadasCampeonato(long campeonatoId) => _rodadaRepository.ListaRodadasCampeonato(campeonatoId);
        public Task AtualizaRodada(RodadaModel req) => _rodadaRepository.AtualizaRodada(req);
        public Task ExcluiRodada(long id) => _rodadaRepository.ExcluiRodada(id);
    }
}
