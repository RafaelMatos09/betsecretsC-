using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IEscalacaoService
    {
        Task<EscalacaoModel> CadastrarEscalacao(EscalacaoModel req);
    }
}
