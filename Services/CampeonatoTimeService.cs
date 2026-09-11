using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class CampeonatoTimeService : ICampeonatoTimeService
    {
        private readonly ICampeonatoTimeRepository _campeonatoTimeRepository;

        public CampeonatoTimeService(ICampeonatoTimeRepository campeonatoTimeRepository)
        {
            _campeonatoTimeRepository = campeonatoTimeRepository;
        }

        public async Task<CampeonatoTimeModel> CadastrarCampeonatoTime(CampeonatoTimeModel req)
        {
            return await _campeonatoTimeRepository.CadastrarCampeonatoTime(req);
        }
        public Task<List<CampeonatoTimeModel>> ListaTimesCampeonato(long campeonatoId) => _campeonatoTimeRepository.ListaTimesCampeonato(campeonatoId);
        public Task AtualizaGrupo(CampeonatoTimeModel req) => _campeonatoTimeRepository.AtualizaGrupo(req);
        public Task ExcluiCampeonatoTime(long campeonatoId, long timeId) => _campeonatoTimeRepository.ExcluiCampeonatoTime(campeonatoId, timeId);
    }
}
