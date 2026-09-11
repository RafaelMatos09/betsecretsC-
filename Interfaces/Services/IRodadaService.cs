using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IRodadaService
    {
        Task<RodadaModel> CadastrarRodada(RodadaModel req);
    }
}
