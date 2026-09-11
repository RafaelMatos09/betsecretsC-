using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IRodadaRepository
    {
        Task<RodadaModel> CadastrarRodada(RodadaModel req);
    }
}
