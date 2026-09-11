using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface ICampeonatoTimeService
    {
        Task<CampeonatoTimeModel> CadastrarCampeonatoTime(CampeonatoTimeModel req);
    }
}
