using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IClassificacaoRepository
    {
        Task<ClassificacaoModel> CadastrarClassificacao(ClassificacaoModel req);
    }
}
