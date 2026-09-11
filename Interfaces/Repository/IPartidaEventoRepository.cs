using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IPartidaEventoRepository
    {
        Task<PartidaEventoModel> CadastrarPartidaEvento(PartidaEventoModel req);
    }
}
