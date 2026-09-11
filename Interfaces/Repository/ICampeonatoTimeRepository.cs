using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ICampeonatoTimeRepository
    {
        Task<CampeonatoTimeModel> CadastrarCampeonatoTime(CampeonatoTimeModel req);
    }
}
