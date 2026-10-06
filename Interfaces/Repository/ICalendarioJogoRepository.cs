using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ICalendarioJogoRepository
    {
        Task<CalendarioJogoModel> Cadastrar(CalendarioJogoModel req);
        Task<List<CalendarioJogoModel>> Listar(long? campeonatoId, long? timeId);
        Task<CalendarioJogoModel?> Consultar(long id);
        Task Confirmar(long id, DateTime dataPrevista, TimeSpan? horarioPrevisto, string? localPrevisto);
        Task VincularPartida(long id, long partidaId);
        Task AtualizarStatus(long id, string status, string? observacoes);
        Task<CalendarioJogoModel> Oficializar(long id);
        Task Excluir(long id);
    }
}
