using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly AppDbContext _context;
        private static int _schemaPronto;

        public RelatorioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RelatorioJogadorModel>> ListarJogadores(long? campeonatoId, long? timeId)
        {
            var query = @"
                        SELECT
                            j.id AS JogadorId,
                            j.nome,
                            j.foto,
                            j.posicao,
                            t.id AS TimeId,
                            t.nome AS TimeNome,
                            p.id AS PartidaId,
                            cj.id AS CalendarioId,
                            p.data_hora AS DataHora,
                            cj.data_prevista AS DataPrevista,
                            cj.horario_previsto AS HorarioPrevisto,
                            cj.status AS StatusCalendario,
                            p.time_casa_id AS TimeCasaId,
                            p.time_visitante_id AS TimeVisitanteId,
                            tc.nome AS TimeCasa,
                            tv.nome AS TimeVisitante,
                            p.gols_casa AS GolsCasa,
                            p.gols_visitante AS GolsVisitante,
                            ej.titular,
                            ej.numero_camisa AS NumeroCamisa,
                            ej.minuto_entrada AS MinutoEntrada,
                            ej.minuto_saida AS MinutoSaida,
                            (
                                SELECT COUNT(*)
                                FROM partida_eventos pe
                                WHERE pe.partida_id = p.id
                                  AND pe.jogador_id = j.id
                                  AND pe.tipo IN ('gol', 'penalti_marcado')
                            ) AS Gols,
                            (
                                SELECT COUNT(*)
                                FROM partida_eventos pe
                                WHERE pe.partida_id = p.id
                                  AND pe.jogador_id = j.id
                                  AND pe.tipo = 'cartao_amarelo'
                            ) AS CartoesAmarelos,
                            (
                                SELECT COUNT(*)
                                FROM partida_eventos pe
                                WHERE pe.partida_id = p.id
                                  AND pe.jogador_id = j.id
                                  AND pe.tipo = 'cartao_vermelho'
                            ) AS CartoesVermelhos
                        FROM escalacao_jogo ej
                        INNER JOIN jogadores j ON j.id = ej.jogador_id
                        INNER JOIN times t ON t.id = ej.time_id
                        INNER JOIN partidas p ON p.id = ej.partida_id
                        INNER JOIN times tc ON tc.id = p.time_casa_id
                        INNER JOIN times tv ON tv.id = p.time_visitante_id
                        LEFT JOIN calendario_jogos cj ON cj.partida_id = p.id
                        WHERE (@CampeonatoId::BIGINT IS NULL OR p.campeonato_id = @CampeonatoId)
                          AND (@TimeId::BIGINT IS NULL OR ej.time_id = @TimeId)
                        ORDER BY j.nome, COALESCE(cj.data_prevista, p.data_hora::date) DESC";

            var dbPara = new DynamicParameters();
            dbPara.Add("@CampeonatoId", campeonatoId);
            dbPara.Add("@TimeId", timeId);

            try
            {
                var linhas = (await _context.GetAllAsync<RelatorioJogadorLinha>(query, dbPara)).ToList();
                return linhas
                    .GroupBy(linha => linha.JogadorId)
                    .Select(grupo =>
                    {
                        var primeiro = grupo.First();
                        var partidas = grupo.Select(linha => new RelatorioJogadorPartidaModel
                        {
                            PartidaId = linha.PartidaId,
                            CalendarioId = linha.CalendarioId,
                            TimeId = linha.TimeId,
                            DataHora = linha.DataHora,
                            DataPrevista = linha.DataPrevista,
                            HorarioPrevisto = linha.HorarioPrevisto,
                            TimeCasaId = linha.TimeCasaId,
                            TimeVisitanteId = linha.TimeVisitanteId,
                            TimeCasa = linha.TimeCasa,
                            TimeVisitante = linha.TimeVisitante,
                            GolsCasa = linha.GolsCasa,
                            GolsVisitante = linha.GolsVisitante,
                            Titular = linha.Titular,
                            NumeroCamisa = linha.NumeroCamisa,
                            MinutoEntrada = linha.MinutoEntrada,
                            MinutoSaida = linha.MinutoSaida,
                            Gols = linha.Gols,
                            CartoesAmarelos = linha.CartoesAmarelos,
                            CartoesVermelhos = linha.CartoesVermelhos,
                            StatusCalendario = linha.StatusCalendario,
                        }).ToList();
                        return new RelatorioJogadorModel
                        {
                            JogadorId = primeiro.JogadorId,
                            Nome = primeiro.Nome,
                            Foto = primeiro.Foto,
                            Posicao = primeiro.Posicao,
                            TimeId = primeiro.TimeId,
                            TimeNome = primeiro.TimeNome,
                            Jogos = partidas.Count,
                            Titulares = partidas.Count(item => item.Titular),
                            Gols = partidas.Sum(item => item.Gols),
                            CartoesAmarelos = partidas.Sum(item => item.CartoesAmarelos),
                            CartoesVermelhos = partidas.Sum(item => item.CartoesVermelhos),
                            Partidas = partidas,
                        };
                    })
                    .OrderByDescending(item => item.Gols)
                    .ThenBy(item => item.Nome)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao montar relatório de jogadores: {ex.Message}", ex);
            }
        }

        public async Task<List<RelatorioEstatisticaModel>> ListarEstatisticas(long? campeonatoId, long? timeId)
        {
            await GarantirTabela();

            var query = @"
                        SELECT
                            p.id AS PartidaId,
                            cj.id AS CalendarioId,
                            p.campeonato_id AS CampeonatoId,
                            p.data_hora AS DataHora,
                            cj.data_prevista AS DataPrevista,
                            cj.horario_previsto AS HorarioPrevisto,
                            COALESCE(p.local, cj.local_previsto) AS Local,
                            COALESCE(p.status, cj.status) AS Status,
                            p.gols_casa AS GolsCasa,
                            p.gols_visitante AS GolsVisitante,
                            p.time_casa_id AS TimeCasaId,
                            tc.nome AS TimeCasa,
                            tc.escudo AS EscudoCasa,
                            p.time_visitante_id AS TimeVisitanteId,
                            tv.nome AS TimeVisitante,
                            tv.escudo AS EscudoVisitante,
                            ec.id AS EstatisticaCasaId,
                            ec.chutes AS ChutesCasa,
                            ec.chutes_gol AS ChutesGolCasa,
                            ec.posse AS PosseCasa,
                            ec.escanteios AS EscanteiosCasa,
                            ec.faltas AS FaltasCasa,
                            ec.impedimentos AS ImpedimentosCasa,
                            ec.cartoes_amarelos AS AmarelosCasa,
                            ec.cartoes_vermelhos AS VermelhosCasa,
                            ec.passes AS PassesCasa,
                            ec.laterais AS LateraisCasa,
                            ev.id AS EstatisticaVisitanteId,
                            ev.chutes AS ChutesVisitante,
                            ev.chutes_gol AS ChutesGolVisitante,
                            ev.posse AS PosseVisitante,
                            ev.escanteios AS EscanteiosVisitante,
                            ev.faltas AS FaltasVisitante,
                            ev.impedimentos AS ImpedimentosVisitante,
                            ev.cartoes_amarelos AS AmarelosVisitante,
                            ev.cartoes_vermelhos AS VermelhosVisitante,
                            ev.passes AS PassesVisitante,
                            ev.laterais AS LateraisVisitante
                        FROM partidas p
                        INNER JOIN times tc ON tc.id = p.time_casa_id
                        INNER JOIN times tv ON tv.id = p.time_visitante_id
                        LEFT JOIN calendario_jogos cj ON cj.partida_id = p.id
                        LEFT JOIN estatistica_partida ec ON ec.partida_id = p.id AND ec.time_id = p.time_casa_id
                        LEFT JOIN estatistica_partida ev ON ev.partida_id = p.id AND ev.time_id = p.time_visitante_id
                        WHERE (@CampeonatoId::BIGINT IS NULL OR p.campeonato_id = @CampeonatoId)
                          AND (
                              @TimeId::BIGINT IS NULL
                              OR p.time_casa_id = @TimeId
                              OR p.time_visitante_id = @TimeId
                          )
                        ORDER BY COALESCE(cj.data_prevista, p.data_hora::date) DESC";

            var dbPara = new DynamicParameters();
            dbPara.Add("@CampeonatoId", campeonatoId);
            dbPara.Add("@TimeId", timeId);

            try
            {
                var linhas = (await _context.GetAllAsync<EstatisticaLinha>(query, dbPara)).ToList();
                var eventos = await ContarEventos(linhas.Select(item => item.PartidaId).Distinct().ToArray());

                return linhas.Select(linha => new RelatorioEstatisticaModel
                {
                    PartidaId = linha.PartidaId,
                    CalendarioId = linha.CalendarioId,
                    CampeonatoId = linha.CampeonatoId,
                    DataHora = linha.DataHora,
                    DataPrevista = linha.DataPrevista,
                    HorarioPrevisto = linha.HorarioPrevisto,
                    Local = linha.Local,
                    Status = linha.Status,
                    Casa = MontarLado(
                        linha.TimeCasaId,
                        linha.TimeCasa,
                        linha.EscudoCasa,
                        linha.GolsCasa ?? 0,
                        linha.EstatisticaCasaId is > 0,
                        linha.ChutesCasa,
                        linha.ChutesGolCasa,
                        linha.PosseCasa,
                        linha.EscanteiosCasa,
                        linha.FaltasCasa,
                        linha.ImpedimentosCasa,
                        linha.AmarelosCasa,
                        linha.VermelhosCasa,
                        linha.PassesCasa,
                        linha.LateraisCasa,
                        eventos,
                        linha.PartidaId),
                    Visitante = MontarLado(
                        linha.TimeVisitanteId,
                        linha.TimeVisitante,
                        linha.EscudoVisitante,
                        linha.GolsVisitante ?? 0,
                        linha.EstatisticaVisitanteId is > 0,
                        linha.ChutesVisitante,
                        linha.ChutesGolVisitante,
                        linha.PosseVisitante,
                        linha.EscanteiosVisitante,
                        linha.FaltasVisitante,
                        linha.ImpedimentosVisitante,
                        linha.AmarelosVisitante,
                        linha.VermelhosVisitante,
                        linha.PassesVisitante,
                        linha.LateraisVisitante,
                        eventos,
                        linha.PartidaId),
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao montar estatísticas das partidas: {ex.Message}", ex);
            }
        }

        public async Task SalvarEstatistica(SalvarEstatisticaRequest req)
        {
            await GarantirTabela();

            var query = @"
                        INSERT INTO estatistica_partida
                        (
                            partida_id, time_id, chutes, chutes_gol, posse, escanteios,
                            faltas, impedimentos, cartoes_amarelos, cartoes_vermelhos, passes, laterais
                        )
                        VALUES
                        (
                            @PartidaId, @TimeId, @Chutes, @ChutesGol, @Posse, @Escanteios,
                            @Faltas, @Impedimentos, @CartoesAmarelos, @CartoesVermelhos, @Passes, @Laterais
                        )
                        ON CONFLICT (partida_id, time_id) DO UPDATE
                        SET chutes = EXCLUDED.chutes,
                            chutes_gol = EXCLUDED.chutes_gol,
                            posse = EXCLUDED.posse,
                            escanteios = EXCLUDED.escanteios,
                            faltas = EXCLUDED.faltas,
                            impedimentos = EXCLUDED.impedimentos,
                            cartoes_amarelos = EXCLUDED.cartoes_amarelos,
                            cartoes_vermelhos = EXCLUDED.cartoes_vermelhos,
                            passes = EXCLUDED.passes,
                            laterais = EXCLUDED.laterais";

            var dbPara = new DynamicParameters();
            dbPara.Add("@PartidaId", req.PartidaId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@Chutes", req.Chutes);
            dbPara.Add("@ChutesGol", req.ChutesGol);
            dbPara.Add("@Posse", req.Posse);
            dbPara.Add("@Escanteios", req.Escanteios);
            dbPara.Add("@Faltas", req.Faltas);
            dbPara.Add("@Impedimentos", req.Impedimentos);
            dbPara.Add("@CartoesAmarelos", req.CartoesAmarelos);
            dbPara.Add("@CartoesVermelhos", req.CartoesVermelhos);
            dbPara.Add("@Passes", req.Passes);
            dbPara.Add("@Laterais", req.Laterais);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar estatística da partida: {ex.Message}", ex);
            }
        }

        private async Task GarantirTabela()
        {
            if (Volatile.Read(ref _schemaPronto) == 1) return;

            var query = @"
                        CREATE TABLE IF NOT EXISTS estatistica_partida (
                            id                  BIGSERIAL PRIMARY KEY,
                            partida_id          BIGINT NOT NULL REFERENCES partidas(id) ON DELETE CASCADE,
                            time_id             BIGINT NOT NULL REFERENCES times(id),
                            chutes              INTEGER NOT NULL DEFAULT 0,
                            chutes_gol          INTEGER NOT NULL DEFAULT 0,
                            posse               NUMERIC(5,2),
                            escanteios          INTEGER NOT NULL DEFAULT 0,
                            faltas              INTEGER NOT NULL DEFAULT 0,
                            impedimentos        INTEGER NOT NULL DEFAULT 0,
                            cartoes_amarelos    INTEGER NOT NULL DEFAULT 0,
                            cartoes_vermelhos   INTEGER NOT NULL DEFAULT 0,
                            passes              INTEGER NOT NULL DEFAULT 0,
                            laterais            INTEGER NOT NULL DEFAULT 0,
                            CONSTRAINT uq_estatistica_partida_time UNIQUE (partida_id, time_id)
                        )";

            await _context.ExecuteAsync(query);
            Volatile.Write(ref _schemaPronto, 1);
        }

        private async Task<List<EventoContagem>> ContarEventos(long[] partidaIds)
        {
            if (partidaIds.Length == 0) return new List<EventoContagem>();

            var query = @"
                        SELECT
                            pe.partida_id AS PartidaId,
                            pe.time_id AS TimeId,
                            pe.tipo,
                            COUNT(*) AS Total
                        FROM partida_eventos pe
                        WHERE pe.partida_id = ANY(@PartidaIds)
                        GROUP BY pe.partida_id, pe.time_id, pe.tipo";

            var dbPara = new DynamicParameters();
            dbPara.Add("@PartidaIds", partidaIds);
            var eventos = await _context.GetAllAsync<EventoContagem>(query, dbPara);
            return eventos.ToList();
        }

        private static EstatisticaLadoModel MontarLado(
            long? timeId,
            string? nome,
            string? escudo,
            int gols,
            bool registrada,
            int? chutes,
            int? chutesGol,
            decimal? posse,
            int? escanteios,
            int? faltas,
            int? impedimentos,
            int? amarelos,
            int? vermelhos,
            int? passes,
            int? laterais,
            List<EventoContagem> eventos,
            long partidaId)
        {
            int Contar(params string[] tipos) =>
                eventos
                    .Where(item => item.PartidaId == partidaId && item.TimeId == timeId && tipos.Contains(item.Tipo))
                    .Sum(item => item.Total);

            return new EstatisticaLadoModel
            {
                TimeId = timeId,
                TimeNome = nome,
                Escudo = escudo,
                Registrada = registrada,
                Gols = gols,
                Chutes = registrada ? chutes ?? 0 : Contar("chute", "chute_gol"),
                ChutesGol = registrada ? chutesGol ?? 0 : Contar("chute_gol"),
                Posse = registrada ? posse : null,
                Escanteios = registrada ? escanteios ?? 0 : Contar("escanteio"),
                Faltas = registrada ? faltas ?? 0 : Contar("falta"),
                Impedimentos = registrada ? impedimentos ?? 0 : Contar("impedimento"),
                CartoesAmarelos = registrada ? amarelos ?? 0 : Contar("cartao_amarelo"),
                CartoesVermelhos = registrada ? vermelhos ?? 0 : Contar("cartao_vermelho"),
                Passes = registrada ? passes ?? 0 : Contar("passe"),
                Laterais = registrada ? laterais ?? 0 : Contar("lateral"),
            };
        }
    }
}
