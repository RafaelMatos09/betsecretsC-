using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IPracaService
    {
        Task<PracaModel> Cadastrar(PracaModel req);
        Task<List<PracaModel>> Listar(long? bairroId);
        Task<PracaModel?> Consultar(long id);
        Task Atualizar(PracaModel req);
        Task Excluir(long id);
    }
}
