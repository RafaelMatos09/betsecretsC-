using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IJogadorTimeRepository
    {
        Task<JogadorTimeModel> CadastrarJogadorTime(JogadorTimeModel req);
    }
}
