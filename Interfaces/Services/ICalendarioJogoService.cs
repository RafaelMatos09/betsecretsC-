using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface ICalendarioJogoService
    {
        Task<CalendarioJogoModel> Cadastrar(CalendarioJogoModel req);
        Task<List<CalendarioJogoModel>> Listar(long? campeonatoId, long? timeId);
        Task<CalendarioJogoModel?> Consultar(long id);
        Task Confirmar(long id, ConfirmarCalendarioRequest req);
        Task VincularPartida(long id, long partidaId);
        Task AtualizarStatus(long id, AtualizarStatusCalendarioRequest req);
        Task<CalendarioJogoModel> Oficializar(long id);
        Task Excluir(long id);
    }
}
