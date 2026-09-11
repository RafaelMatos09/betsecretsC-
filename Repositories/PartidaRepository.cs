using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class PartidaRepository : IPartidaRepository
    {
        private readonly AppDbContext _context;

        public PartidaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PartidaModel> CadastrarPartida(PartidaModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", req.CampeonatoId);
            dbPara.Add("@RodadaId", req.RodadaId);
            dbPara.Add("@TimeCasaId", req.TimeCasaId);
            dbPara.Add("@TimeVisitanteId", req.TimeVisitanteId);
            dbPara.Add("@Local", req.Local);
            dbPara.Add("@DataHora", req.DataHora);
            dbPara.Add("@Status", req.Status);
            dbPara.Add("@Arbitro", req.Arbitro);

            var query = @"
                        INSERT INTO partidas
                        (
                            campeonato_id,
                            rodada_id,
                            time_casa_id,
                            time_visitante_id,
                            local,
                            data_hora,
                            status,
                            arbitro
                        )
                        VALUES
                        (
                            @CampeonatoId,
                            @RodadaId,
                            @TimeCasaId,
                            @TimeVisitanteId,
                            @Local,
                            @DataHora,
                            @Status,
                            @Arbitro
                        )
                        RETURNING id";

           

            try
            {
                var id = await _context.ExecuteAsync(query, dbPara);
                req.Id = id;

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar partida: {ex.Message}", ex);
            }
        }
    }
}
