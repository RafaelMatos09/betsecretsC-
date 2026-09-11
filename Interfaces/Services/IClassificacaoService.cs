using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IClassificacaoService
    {
        Task<ClassificacaoModel> CadastrarClassificacao(ClassificacaoModel req);
        Task<List<ClassificacaoModel>> ListaClassificacao(long campeonatoId); 
        Task RecalculaPosicoes(long campeonatoId);
    }
}
