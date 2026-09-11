using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class BairroService : IBairroService
    {
        private readonly IBairroRepository _bairroRepository;

        public BairroService(IBairroRepository bairroRepository)
        {
            _bairroRepository = bairroRepository;
        }
        
        public async Task<BairroModel> CadastraBairro(BairroModel req)
        {
            return await _bairroRepository.CadastraBairro(req);
        }

        public async Task<List<BairroModel>> ListaBairros()
        {
            return await _bairroRepository.ListaBairros();
        }
        public Task<BairroModel?> ConsultaBairro(int id) => _bairroRepository.ConsultaBairro(id);
        public Task<List<BairroModel>> ListaBairrosPorCidade(string cidade) => _bairroRepository.ListaBairrosPorCidade(cidade);
        public Task AtualizaBairro(BairroModel req) => _bairroRepository.AtualizaBairro(req);
        public Task ExcluiBairro(int id) => _bairroRepository.ExcluiBairro(id);
    }
}
