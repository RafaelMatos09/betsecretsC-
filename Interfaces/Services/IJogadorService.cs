using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IJogadorService
    {
        Task<JogadorModel> CadastraJogador(JogadorModel req);
    }
}
