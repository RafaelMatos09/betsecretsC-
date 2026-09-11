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
    }
}
