using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _repository;

        public RelatorioService(IRelatorioRepository repository)
        {
            _repository = repository;
        }

        public Task<List<RelatorioJogadorModel>> ListarJogadores(long? campeonatoId, long? timeId) =>
            _repository.ListarJogadores(campeonatoId, timeId);

        public Task<List<RelatorioEstatisticaModel>> ListarEstatisticas(long? campeonatoId, long? timeId) =>
            _repository.ListarEstatisticas(campeonatoId, timeId);

        public Task SalvarEstatistica(SalvarEstatisticaRequest req)
        {
            if (req.PartidaId <= 0 || req.TimeId <= 0)
                throw new Exception("Informe a partida e o time da estatística.");
            if (req.Posse is < 0 or > 100)
                throw new Exception("A posse de bola precisa ficar entre 0 e 100.");

            return _repository.SalvarEstatistica(req);
        }
    }
}
