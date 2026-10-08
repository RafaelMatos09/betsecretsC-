using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class CalendarioJogoRepository : ICalendarioJogoRepository
    {
        private readonly AppDbContext _context;

        public CalendarioJogoRepository(AppDbContext context)
        {
            _context = context;
        }

        private const string SelectAgenda = @"
                        SELECT
                            cj.id,
                            cj.campeonato_id AS CampeonatoId,
                            cj.rodada_id AS RodadaId,
                            cj.partida_id AS PartidaId,
                            cj.time_casa_id AS TimeCasaId,
                            cj.time_visitante_id AS TimeVisitanteId,
                            cj.data_prevista AS DataPrevista,
                            cj.horario_previsto AS HorarioPrevisto,
                            cj.local_previsto AS LocalPrevisto,
                            cj.praca_id AS PracaId,
                            p.nome AS PracaNome,
                            p.endereco AS PracaEndereco,
                            p.latitude AS PracaLatitude,
                            p.longitude AS PracaLongitude,
                            cj.status,
                            cj.observacoes,
                            tc.nome AS TimeCasa,
                            tc.sigla AS SiglaCasa,
                            tc.escudo AS EscudoCasa,
                            tv.nome AS TimeVisitante,
                            tv.sigla AS SiglaVisitante,
                            tv.escudo AS EscudoVisitante,
                            r.numero AS RodadaNumero,
                            c.nome AS CampeonatoNome
                        FROM calendario_jogos cj
                        INNER JOIN times tc ON tc.id = cj.time_casa_id
                        INNER JOIN times tv ON tv.id = cj.time_visitante_id
                        LEFT JOIN rodadas r ON r.id = cj.rodada_id
                        LEFT JOIN campeonatos c ON c.id = cj.campeonato_id
                        LEFT JOIN pracas p ON p.id = cj.praca_id";

        public async Task<CalendarioJogoModel> Cadastrar(CalendarioJogoModel req)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("@CampeonatoId", req.CampeonatoId);
            dbPara.Add("@RodadaId", req.RodadaId);
            dbPara.Add("@TimeCasaId", req.TimeCasaId);
            dbPara.Add("@TimeVisitanteId", req.TimeVisitanteId);
            dbPara.Add("@DataPrevista", req.DataPrevista);
            dbPara.Add("@HorarioPrevisto", req.HorarioPrevisto);
            dbPara.Add("@LocalPrevisto", req.LocalPrevisto);
            dbPara.Add("@PracaId", req.PracaId);
            dbPara.Add("@Status", string.IsNullOrWhiteSpace(req.Status) ? "previsto" : req.Status);
            dbPara.Add("@Observacoes", req.Observacoes);

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
                            praca_id,
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
                            @PracaId,
                            @Status,
                            @Observacoes
                        )
                        RETURNING id";

            try
            {
                var id = await _context.ExecuteScalarAsync<long>(query, dbPara);
                return await Consultar(id) ?? req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar jogo no calendário: {ex.Message}", ex);
            }
        }

        public async Task<List<CalendarioJogoModel>> Listar(long? campeonatoId, long? timeId)
        {
            var query = SelectAgenda + @"
                        WHERE (@CampeonatoId::BIGINT IS NULL OR cj.campeonato_id = @CampeonatoId)
                          AND (
                              @TimeId::BIGINT IS NULL
                              OR cj.time_casa_id = @TimeId
                              OR cj.time_visitante_id = @TimeId
                          )
                        ORDER BY cj.data_prevista, cj.horario_previsto NULLS LAST";

            var dbPara = new DynamicParameters();
            dbPara.Add("@CampeonatoId", campeonatoId);
            dbPara.Add("@TimeId", timeId);

            try
            {
                var jogos = await _context.GetAllAsync<CalendarioJogoModel>(query, dbPara);
                return jogos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar calendário de jogos: {ex.Message}", ex);
            }
        }

        public async Task<CalendarioJogoModel?> Consultar(long id)
        {
            var query = SelectAgenda + " WHERE cj.id = @Id";
            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);

            try
            {
                var jogos = await _context.GetAllAsync<CalendarioJogoModel>(query, dbPara);
                return jogos.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar jogo do calendário: {ex.Message}", ex);
            }
        }

        public async Task Confirmar(long id, DateTime dataPrevista, TimeSpan? horarioPrevisto, string? localPrevisto, long? pracaId)
        {
            var query = @"
                        UPDATE calendario_jogos
                        SET data_prevista = @DataPrevista,
                            horario_previsto = @HorarioPrevisto,
                            local_previsto = @LocalPrevisto,
                            praca_id = COALESCE(@PracaId, praca_id),
                            status = 'confirmado',
                            updated_at = NOW()
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);
            dbPara.Add("@DataPrevista", dataPrevista);
            dbPara.Add("@HorarioPrevisto", horarioPrevisto);
            dbPara.Add("@LocalPrevisto", localPrevisto);
            dbPara.Add("@PracaId", pracaId);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao confirmar jogo do calendário: {ex.Message}", ex);
            }
        }

        public async Task VincularPartida(long id, long partidaId)
        {
            var query = @"
                        UPDATE calendario_jogos
                        SET partida_id = @PartidaId,
                            status = 'confirmado',
                            updated_at = NOW()
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);
            dbPara.Add("@PartidaId", partidaId);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao vincular partida ao calendário: {ex.Message}", ex);
            }
        }

        public async Task AtualizarStatus(long id, string status, string? observacoes)
        {
            var query = @"
                        UPDATE calendario_jogos
                        SET status = @Status,
                            observacoes = @Observacoes,
                            updated_at = NOW()
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);
            dbPara.Add("@Status", status);
            dbPara.Add("@Observacoes", observacoes);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar status do calendário: {ex.Message}", ex);
            }
        }

        public async Task<CalendarioJogoModel> Oficializar(long id)
        {
            var atual = await Consultar(id) ?? throw new Exception("Jogo do calendário não encontrado.");
            if (atual.PartidaId is > 0)
                return atual;

            var dataHora = (atual.DataPrevista ?? DateTime.Today).Date;
            if (atual.HorarioPrevisto.HasValue)
                dataHora = dataHora.Add(atual.HorarioPrevisto.Value);

            var dbPara = new DynamicParameters();
            dbPara.Add("@CampeonatoId", atual.CampeonatoId);
            dbPara.Add("@RodadaId", atual.RodadaId);
            dbPara.Add("@TimeCasaId", atual.TimeCasaId);
            dbPara.Add("@TimeVisitanteId", atual.TimeVisitanteId);
            dbPara.Add("@Local", atual.PracaNome ?? atual.LocalPrevisto);
            dbPara.Add("@DataHora", dataHora);

            var insert = @"
                        INSERT INTO partidas
                        (
                            campeonato_id,
                            rodada_id,
                            time_casa_id,
                            time_visitante_id,
                            local,
                            data_hora,
                            status
                        )
                        VALUES
                        (
                            @CampeonatoId,
                            @RodadaId,
                            @TimeCasaId,
                            @TimeVisitanteId,
                            @Local,
                            @DataHora,
                            'agendada'
                        )
                        RETURNING id";

            try
            {
                var partidaId = await _context.ExecuteScalarAsync<long>(insert, dbPara);
                await VincularPartida(id, partidaId);
                return await Consultar(id) ?? atual;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao oficializar jogo do calendário: {ex.Message}", ex);
            }
        }

        public async Task Excluir(long id)
        {
            var query = "DELETE FROM calendario_jogos WHERE id = @Id";
            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir jogo do calendário: {ex.Message}", ex);
            }
        }
    }
}
