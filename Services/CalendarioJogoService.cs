using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class CalendarioJogoService : ICalendarioJogoService
    {
        private static readonly string[] StatusPermitidos =
        [
            "previsto",
            "confirmado",
            "adiado",
            "cancelado",
            "realizado"
        ];

        private readonly ICalendarioJogoRepository _repository;

        public CalendarioJogoService(ICalendarioJogoRepository repository)
        {
            _repository = repository;
        }

        public Task<CalendarioJogoModel> Cadastrar(CalendarioJogoModel req)
        {
            if (req.CampeonatoId is not > 0)
                throw new Exception("Informe o campeonato do jogo.");
            if (req.TimeCasaId is not > 0 || req.TimeVisitanteId is not > 0)
                throw new Exception("Informe os dois times do jogo.");
            if (req.TimeCasaId == req.TimeVisitanteId)
                throw new Exception("O time da casa e o visitante precisam ser diferentes.");
            if (req.DataPrevista is null)
                throw new Exception("Informe a data prevista do jogo.");

            req.Status = string.IsNullOrWhiteSpace(req.Status) ? "previsto" : req.Status.Trim().ToLowerInvariant();
            if (!StatusPermitidos.Contains(req.Status))
                throw new Exception("Status do calendário inválido.");

            return _repository.Cadastrar(req);
        }

        public Task<List<CalendarioJogoModel>> Listar(long? campeonatoId, long? timeId) =>
            _repository.Listar(campeonatoId, timeId);

        public Task<CalendarioJogoModel?> Consultar(long id) => _repository.Consultar(id);

        public Task Confirmar(long id, ConfirmarCalendarioRequest req)
        {
            if (req.DataPrevista == default)
                throw new Exception("Informe a data do jogo.");

            return _repository.Confirmar(id, req.DataPrevista, req.HorarioPrevisto, req.LocalPrevisto, req.PracaId);
        }

        public Task VincularPartida(long id, long partidaId)
        {
            if (partidaId <= 0)
                throw new Exception("Informe a partida a vincular.");

            return _repository.VincularPartida(id, partidaId);
        }

        public Task AtualizarStatus(long id, AtualizarStatusCalendarioRequest req)
        {
            var status = (req.Status ?? string.Empty).Trim().ToLowerInvariant();
            if (!StatusPermitidos.Contains(status))
                throw new Exception("Status do calendário inválido.");

            return _repository.AtualizarStatus(id, status, req.Observacoes);
        }

        public async Task<CalendarioJogoModel> Oficializar(long id)
        {
            var atual = await _repository.Consultar(id) ?? throw new Exception("Jogo do calendário não encontrado.");
            if (atual.Status is "cancelado" or "realizado")
                throw new Exception("Não é possível oficializar um jogo cancelado ou já realizado.");

            return await _repository.Oficializar(id);
        }

        public Task Excluir(long id) => _repository.Excluir(id);
    }
}
