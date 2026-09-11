using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class CampeonatoService : ICampeonatoService
    {
        private readonly ICampeonatoRepository _campeonatoRepository;
        public CampeonatoService(ICampeonatoRepository campeonatoRepository)
        {
            _campeonatoRepository = campeonatoRepository;
        }
        public async Task<CampeonatoModel> CadastroCampeonato(CampeonatoModel req)
        {
            return await _campeonatoRepository.CadastroCampeonato(req);
        }
        public Task<CampeonatoModel?> ConsultaCampeonato(long id) => _campeonatoRepository.ConsultaCampeonato(id);
        public Task<List<CampeonatoModel>> ListaCampeonatos() => _campeonatoRepository.ListaCampeonatos();
        public Task AtualizaCampeonato(CampeonatoModel req) => _campeonatoRepository.AtualizaCampeonato(req);
        public Task ExcluiCampeonato(long id) => _campeonatoRepository.ExcluiCampeonato(id);
    }
}
