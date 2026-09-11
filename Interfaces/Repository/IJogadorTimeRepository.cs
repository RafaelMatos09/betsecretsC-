using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IJogadorTimeRepository
    {
        Task<JogadorTimeModel> CadastrarJogadorTime(JogadorTimeModel req);
        Task<List<JogadorTimeModel>> ListaElenco(long timeId);
        Task<List<JogadorTimeModel>> ListaHistoricoJogador(long jogadorId);
        Task EncerraVinculo(long id, DateTime dataFim);
    }
}
