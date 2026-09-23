using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IEscalacaoRepository
    {
        Task<EscalacaoModel> CadastrarEscalacao(EscalacaoModel req);
    }
}
