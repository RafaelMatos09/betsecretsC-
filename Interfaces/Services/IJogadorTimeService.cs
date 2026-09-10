using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IJogadorTimeService
    {
        Task<JogadorTimeModel> CadastrarJogadorTime(JogadorTimeModel req);
    }
}
