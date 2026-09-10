using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ITimeRepository
    {
        Task<TimesModel> CadastrarTime(TimesModel req);
        Task<List<TimesModel>> ListaTimes();
        Task<List<TimesDetalheModel>> ConsultaTimesDetalhes(string? id = null);
    }
}
