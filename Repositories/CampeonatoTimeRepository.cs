using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class CampeonatoTimeRepository : ICampeonatoTimeRepository
    {
        private readonly AppDbContext _context;
        public CampeonatoTimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CampeonatoTimeModel> CadastrarCampeonatoTime(CampeonatoTimeModel req)
        {
            var query = @"
                        INSERT INTO campeonato_times
                        (
                            campeonato_id,
                            time_id,
                            grupo
                        )
                        VALUES
                        (
                            @CampeonatoId,
                            @TimeId,
                            @Grupo
                        )
                        ON CONFLICT (campeonato_id, time_id) DO NOTHING
                        RETURNING id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", req.CampeonatoId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@Grupo", req.Grupo);

            try
            {
                var id = await _context.ExecuteAsync(query, dbPara);
                req.Id = id;               

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar CampeonatoTime: {ex.Message}", ex);
            }
        }
    }
}
