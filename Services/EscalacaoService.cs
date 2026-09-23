using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class EscalacaoService : IEscalacaoService
    {
        private readonly IEscalacaoRepository _escalacaoRepository;

        public EscalacaoService(IEscalacaoRepository escalacaoRepository)
        {
            _escalacaoRepository = escalacaoRepository;
        }

        public async Task<EscalacaoModel> CadastrarEscalacao(EscalacaoModel req)
        {
            return await _escalacaoRepository.CadastrarEscalacao(req);
        }
    }
}
