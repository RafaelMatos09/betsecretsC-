using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class ClassificacaoService : IClassificacaoService
    {
        private readonly IClassificacaoRepository _classificacaoRepository;

        public ClassificacaoService(IClassificacaoRepository classificacaoRepository)
        {
            _classificacaoRepository = classificacaoRepository;
        }

        public async Task<ClassificacaoModel> CadastrarClassificacao(ClassificacaoModel req)
        {
            return await _classificacaoRepository.CadastrarClassificacao(req);
        }
        public Task<List<ClassificacaoModel>> ListaClassificacao(long campeonatoId) => _classificacaoRepository.ListaClassificacao(campeonatoId);
        public Task RecalculaPosicoes(long campeonatoId) => _classificacaoRepository.RecalculaPosicoes(campeonatoId);
    }
}
