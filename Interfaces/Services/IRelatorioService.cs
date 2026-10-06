using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IRelatorioService
    {
        Task<List<RelatorioJogadorModel>> ListarJogadores(long? campeonatoId, long? timeId);
        Task<List<RelatorioEstatisticaModel>> ListarEstatisticas(long? campeonatoId, long? timeId);
        Task SalvarEstatistica(SalvarEstatisticaRequest req);
    }
}
