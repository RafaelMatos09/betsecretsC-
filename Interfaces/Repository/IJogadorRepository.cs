using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IJogadorRepository
    {
        Task<JogadorModel> CadastraJogador(JogadorModel req);
        Task<JogadorModel?> ConsultaJogador(long id);
        Task<List<JogadorModel>> BuscaJogadores(string nome);
        Task AtualizaJogador(JogadorModel req);
        Task ExcluiJogador(long id);
    }
}
