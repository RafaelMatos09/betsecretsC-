using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IEscalacaoJogoService
    {
        Task<EscalacaoJogoModel> Salvar(EscalacaoJogoModel req);
        Task SalvarTime(SalvarEscalacaoTimeRequest req);
        Task<List<EscalacaoJogoModel>> ListarPorTime(long partidaId, long timeId);
        Task<List<EscalacaoJogoModel>> ListarPorPartida(long partidaId);
        Task RegistrarSaida(long id, short minuto);
        Task RegistrarEntrada(long id, short minuto);
        Task<int> ContarJogosTitular(long jogadorId, long campeonatoId);
        Task Excluir(long id);
    }
}
