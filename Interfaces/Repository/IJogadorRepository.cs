using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IJogadorRepository
    {
        Task<JogadorModel> CadastraJogador(JogadorModel req);
    }
}
