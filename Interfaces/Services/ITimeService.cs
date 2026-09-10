using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface ITimeService
    {
        Task<TimesModel> CadastrarTime(TimesModel req);
    }
}
