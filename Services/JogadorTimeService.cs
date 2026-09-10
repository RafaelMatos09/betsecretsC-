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
    }
}
