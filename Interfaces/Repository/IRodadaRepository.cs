using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IRodadaRepository
    {
        Task<RodadaModel> CadastrarRodada(RodadaModel req);
        Task<List<RodadaModel>> ListaRodadasCampeonato(long campeonatoId);
        Task AtualizaRodada(RodadaModel req);
        Task ExcluiRodada(long id);
    }
}
