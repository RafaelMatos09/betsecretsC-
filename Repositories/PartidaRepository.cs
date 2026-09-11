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

        
        public async Task<PartidaModel?> ConsultaPartida(long id)
        {
            var query = @"
                        SELECT
                            p.id,
                            p.data_hora AS DataHora,
                            p.local,
                            p.status,
                            p.arbitro,
                            p.gols_casa AS GolsCasa,
                            p.gols_visitante AS GolsVisitante,
                            tc.nome AS TimeCasa,
                            tc.escudo AS EscudoCasa,
                            tv.nome AS TimeVisitante,
                            tv.escudo AS EscudoVisitante,
                            r.numero AS RodadaNumero
                        FROM partidas p
                        INNER JOIN times tc ON tc.id = p.time_casa_id
                        INNER JOIN times tv ON tv.id = p.time_visitante_id
                        LEFT JOIN rodadas r ON r.id = p.rodada_id
                        WHERE p.id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            try
            {
                var partida = await _context.GetAllAsync<PartidaModel>(
                    query,
                    dbPara
                );

                return partida.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao consultar partida: {ex.Message}",
                    ex
                );
            }
        }


        public async Task<List<PartidaModel>> ListaPartidasCampeonato(long campeonatoId, long? rodadaId)
        {
            var query = @"
                        SELECT
                            p.id,
                            p.data_hora AS DataHora,
                            p.status,
                            p.gols_casa AS GolsCasa,
                            p.gols_visitante AS GolsVisitante,
                            tc.nome AS TimeCasa,
                            tv.nome AS TimeVisitante
                        FROM partidas p
                        INNER JOIN times tc ON tc.id = p.time_casa_id
                        INNER JOIN times tv ON tv.id = p.time_visitante_id
                        WHERE p.campeonato_id = @CampeonatoId
                          AND (
                              @RodadaId::BIGINT IS NULL
                              OR p.rodada_id = @RodadaId
                          )
                        ORDER BY p.data_hora";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", campeonatoId);
            dbPara.Add("@RodadaId", rodadaId);

            try
            {
                var partidas = await _context.GetAllAsync<PartidaModel>(
                    query,
                    dbPara
                );

                return partidas.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar partidas do campeonato: {ex.Message}",
                    ex
                );
            }
        }


        public async Task RegistraResultado(long id, int golsCasa, int golsVisitante)
        {
            var query = @"
                        UPDATE partidas
                        SET
                            gols_casa = @GolsCasa,
                            gols_visitante = @GolsVisitante,
                            status = 'encerrada',
                            updated_at = NOW()
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);
            dbPara.Add("@GolsCasa", golsCasa);
            dbPara.Add("@GolsVisitante", golsVisitante);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao registrar resultado da partida: {ex.Message}",
                    ex
                );
            }
        }


        public async Task ReagendaPartida(long id, DateTime dataHora)
        {
            var query = @"
                        UPDATE partidas
                        SET
                            data_hora = @DataHora,
                            status = 'adiada'
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);
            dbPara.Add("@DataHora", dataHora);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao reagendar partida: {ex.Message}",
                    ex
                );
            }
        }


        public async Task ExcluiPartida(long id)
        {
            var query = @"
                        DELETE FROM partidas
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao excluir partida: {ex.Message}",
                    ex
                );
            }
        }

    }
}
