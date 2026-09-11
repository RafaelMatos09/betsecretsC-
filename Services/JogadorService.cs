using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class JogadorService : IJogadorService
    {
        private readonly IJogadorRepository _jogadorRepository;

        public JogadorService(IJogadorRepository jogadorRepository)
        {
            _jogadorRepository = jogadorRepository;
        }

        public async Task<JogadorModel> CadastraJogador(JogadorModel req)
        {
            return await _jogadorRepository.CadastraJogador(req);
        }
        public async Task<JogadorModel?> ConsultaJogador(long id)
        {
            return await _jogadorRepository.ConsultaJogador(id);
        }

        public async Task<List<JogadorModel>> BuscaJogadores(string nome)
        {
            return await _jogadorRepository.BuscaJogadores(nome);
        }

        public async Task AtualizaJogador(JogadorModel req)
        {
            await _jogadorRepository.AtualizaJogador(req);
        }

        public async Task ExcluiJogador(long id)
        {
            await _jogadorRepository.ExcluiJogador(id);
        }
    }
}
