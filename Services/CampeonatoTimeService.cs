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
    }
}
