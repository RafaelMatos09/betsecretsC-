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

       
        public async Task<List<CampeonatoTimeModel>> ListaTimesCampeonato(long campeonatoId)
        {
            var query = @"
                        SELECT
                            ct.id,
                            t.id AS TimeId,
                            t.nome,
                            t.escudo,
                            ct.grupo
                        FROM campeonato_times ct
                        INNER JOIN times t ON t.id = ct.time_id
                        WHERE ct.campeonato_id = @CampeonatoId
                        ORDER BY
                            ct.grupo,
                            t.nome";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", campeonatoId);

            try
            {
                var times = await _context.GetAllAsync<CampeonatoTimeModel>(
                    query,
                    dbPara
                );

                return times.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar times do campeonato: {ex.Message}",
                    ex
                );
            }
        }


        public async Task AtualizaGrupo(CampeonatoTimeModel req)
        {
            var query = @"
                        UPDATE campeonato_times
                        SET
                            grupo = @Grupo
                        WHERE campeonato_id = @CampeonatoId
                          AND time_id = @TimeId";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", req.CampeonatoId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@Grupo", req.Grupo);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao atualizar grupo: {ex.Message}",
                    ex
                );
            }
        }


        public async Task ExcluiCampeonatoTime(long campeonatoId, long timeId)
        {
            var query = @"
                        DELETE FROM campeonato_times
                        WHERE campeonato_id = @CampeonatoId
                          AND time_id = @TimeId";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", campeonatoId);
            dbPara.Add("@TimeId", timeId);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao excluir time do campeonato: {ex.Message}",
                    ex
                );
            }
        }


    }
}
