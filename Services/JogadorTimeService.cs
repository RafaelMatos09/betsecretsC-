using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class JogadorTimeService : IJogadorTimeService
    {
        private readonly IJogadorTimeRepository _jogadorTimeRepository;

        public JogadorTimeService(IJogadorTimeRepository jogadorTimeRepository)
        {
            _jogadorTimeRepository = jogadorTimeRepository;
        }

        public async Task<JogadorTimeModel> CadastrarJogadorTime(JogadorTimeModel req)
        {
            return await _jogadorTimeRepository.CadastrarJogadorTime(req);
        }
        public Task<List<JogadorTimeModel>> ListaElenco(long timeId) => _jogadorTimeRepository.ListaElenco(timeId);
        public Task<List<JogadorTimeModel>> ListaHistoricoJogador(long jogadorId) => _jogadorTimeRepository.ListaHistoricoJogador(jogadorId);
        public Task EncerraVinculo(long id, DateTime dataFim) => _jogadorTimeRepository.EncerraVinculo(id, dataFim);
    }
}
