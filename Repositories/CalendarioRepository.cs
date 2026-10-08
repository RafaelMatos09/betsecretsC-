using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class CalendarioRepository : ICalendarioRepository
    {
        private readonly AppDbContext _context;
        public CalendarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CalendarioJogosModel> CadastrarCalendarioJogos(CalendarioJogosModel req)
        {
            var query = @"
                        INSERT INTO calendario_jogos 
                        (
                            campeonato_id,
                            rodada_id,
                            time_casa_id,
                            time_visitante_id,
                            data_prevista,
                            horario_previsto,
                            local_previsto,
                            status,
                            observacoes
                        )
                        VALUES 
                        (
                            @CampeonatoId,
                            @RodadaId,
                            @TimeCasaId,
                            @TimeVisitanteId,
                            @DataPrevista,
                            @HorarioPrevisto,
                            @LocalPrevisto,
                            'previsto',
                            @Observacoes
                        )
                        RETURNING id";

            try
            {
                var dbPara = new DynamicParameters();

                dbPara.Add("@CampeonatoId", req.CampeonatoId);
                dbPara.Add("@RodadaId", req.RodadaId);
                dbPara.Add("@TimeCasaId", req.TimeCasaId);
                dbPara.Add("@TimeVisitanteId", req.TimeVisitanteId);
                dbPara.Add("@DataPrevista", req.DataPrevista);
                dbPara.Add("@HorarioPrevisto", req.HorarioPrevisto);
                dbPara.Add("@LocalPrevisto", req.LocalPrevisto);
                dbPara.Add("@Observacoes", req.Observacoes);

                var id = await _context.ExecuteAsync(query, dbPara);

                req.Id = id;
                req.Status = "previsto";

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao cadastrar calendário de jogos: {ex.Message}", ex);
            }
        }
    }
}
