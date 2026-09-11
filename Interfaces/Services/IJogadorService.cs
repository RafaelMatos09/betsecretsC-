using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IJogadorService
    {
        Task<JogadorModel> CadastraJogador(JogadorModel req);
        Task<JogadorModel?> ConsultaJogador(long id);
        Task<List<JogadorModel>> BuscaJogadores(string nome);
        Task AtualizaJogador(JogadorModel req);
        Task ExcluiJogador(long id);
    }
}
