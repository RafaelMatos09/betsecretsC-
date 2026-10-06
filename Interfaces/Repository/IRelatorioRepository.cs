using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IRelatorioRepository
    {
        Task<List<RelatorioJogadorModel>> ListarJogadores(long? campeonatoId, long? timeId);
        Task<List<RelatorioEstatisticaModel>> ListarEstatisticas(long? campeonatoId, long? timeId);
        Task SalvarEstatistica(SalvarEstatisticaRequest req);
    }
}
