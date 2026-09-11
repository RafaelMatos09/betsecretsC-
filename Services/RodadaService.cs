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
    }
}
