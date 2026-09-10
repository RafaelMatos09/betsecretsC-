using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ITimeRepository
    {
        Task<TimesModel> CadastrarTime(TimesModel req);
    }
}
