using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface ITimeService
    {
        Task<TimesModel> CadastrarTime(TimesModel req);
        Task<List<TimesModel>> ListaTimes();
        Task<List<TimesDetalheModel>> ConsultaTimesDetalhes(string? id = null);
    }
}
