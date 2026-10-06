using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class EscalacaoJogoService : IEscalacaoJogoService
    {
        private readonly IEscalacaoJogoRepository _repository;

        public EscalacaoJogoService(IEscalacaoJogoRepository repository)
        {
            _repository = repository;
        }

        public Task<EscalacaoJogoModel> Salvar(EscalacaoJogoModel req)
        {
            ValidarItem(req);
            return _repository.Salvar(req);
        }

        public Task SalvarTime(SalvarEscalacaoTimeRequest req)
        {
            if (req.PartidaId <= 0)
                throw new Exception("Informe a partida da escalação.");
            if (req.TimeId <= 0)
                throw new Exception("Informe o time da escalação.");

            var jogadores = req.Jogadores ?? new List<EscalacaoJogoModel>();
            foreach (var jogador in jogadores)
            {
                jogador.PartidaId = req.PartidaId;
                jogador.TimeId = req.TimeId;
                ValidarItem(jogador);
            }

            return _repository.SalvarTime(req.PartidaId, req.TimeId, jogadores);
        }

        public Task<List<EscalacaoJogoModel>> ListarPorTime(long partidaId, long timeId) =>
            _repository.ListarPorTime(partidaId, timeId);

        public Task<List<EscalacaoJogoModel>> ListarPorPartida(long partidaId) =>
            _repository.ListarPorPartida(partidaId);

        public Task RegistrarSaida(long id, short minuto) => _repository.RegistrarSaida(id, minuto);

        public Task RegistrarEntrada(long id, short minuto) => _repository.RegistrarEntrada(id, minuto);

        public Task<int> ContarJogosTitular(long jogadorId, long campeonatoId) =>
            _repository.ContarJogosTitular(jogadorId, campeonatoId);

        public Task Excluir(long id) => _repository.Excluir(id);

        private static void ValidarItem(EscalacaoJogoModel req)
        {
            if (req.PartidaId is not > 0)
                throw new Exception("Informe a partida da escalação.");
            if (req.JogadorId is not > 0)
                throw new Exception("Informe o jogador da escalação.");
            if (req.TimeId is not > 0)
                throw new Exception("Informe o time da escalação.");
        }
    }
}
