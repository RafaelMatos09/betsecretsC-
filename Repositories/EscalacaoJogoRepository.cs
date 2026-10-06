using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class EscalacaoJogoRepository : IEscalacaoJogoRepository
    {
        private readonly AppDbContext _context;

        public EscalacaoJogoRepository(AppDbContext context)
        {
            _context = context;
        }

        private const string SelectEscalacao = @"
                        SELECT
                            ej.id,
                            ej.partida_id AS PartidaId,
                            j.id AS JogadorId,
                            ej.time_id AS TimeId,
                            j.nome,
                            j.foto,
                            ej.titular,
                            ej.numero_camisa AS NumeroCamisa,
                            ej.posicao,
                            ej.minuto_entrada AS MinutoEntrada,
                            ej.minuto_saida AS MinutoSaida
                        FROM escalacao_jogo ej
                        INNER JOIN jogadores j ON j.id = ej.jogador_id";

        public async Task<EscalacaoJogoModel> Salvar(EscalacaoJogoModel req)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("@PartidaId", req.PartidaId);
            dbPara.Add("@JogadorId", req.JogadorId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@Titular", req.Titular);
            dbPara.Add("@NumeroCamisa", req.NumeroCamisa);
            dbPara.Add("@Posicao", req.Posicao);
            dbPara.Add("@MinutoEntrada", req.MinutoEntrada);

            var query = @"
                        INSERT INTO escalacao_jogo
                        (
                            partida_id,
                            jogador_id,
                            time_id,
                            titular,
                            numero_camisa,
                            posicao,
                            minuto_entrada
                        )
                        VALUES
                        (
                            @PartidaId,
                            @JogadorId,
                            @TimeId,
                            @Titular,
                            @NumeroCamisa,
                            @Posicao,
                            @MinutoEntrada
                        )
                        ON CONFLICT (partida_id, jogador_id) DO UPDATE
                        SET titular = EXCLUDED.titular,
                            numero_camisa = EXCLUDED.numero_camisa,
                            posicao = EXCLUDED.posicao
                        RETURNING id";

            try
            {
                var id = await _context.ExecuteScalarAsync<long>(query, dbPara);
                req.Id = id;
                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar escalação: {ex.Message}", ex);
            }
        }

        public async Task SalvarTime(long partidaId, long timeId, List<EscalacaoJogoModel> jogadores)
        {
            var jogadorIds = new List<long>();

            foreach (var jogador in jogadores)
            {
                if (jogador.JogadorId is not > 0)
                    continue;

                jogador.PartidaId = partidaId;
                jogador.TimeId = timeId;
                await Salvar(jogador);
                jogadorIds.Add(jogador.JogadorId.Value);
            }

            var dbPara = new DynamicParameters();
            dbPara.Add("@PartidaId", partidaId);
            dbPara.Add("@TimeId", timeId);
            dbPara.Add("@JogadorIds", jogadorIds.ToArray());

            var query = jogadorIds.Count == 0
                ? @"DELETE FROM escalacao_jogo WHERE partida_id = @PartidaId AND time_id = @TimeId"
                : @"DELETE FROM escalacao_jogo
                    WHERE partida_id = @PartidaId
                      AND time_id = @TimeId
                      AND NOT (jogador_id = ANY(@JogadorIds))";

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao sincronizar escalação do time: {ex.Message}", ex);
            }
        }

        public async Task<List<EscalacaoJogoModel>> ListarPorTime(long partidaId, long timeId)
        {
            var query = SelectEscalacao + @"
                        WHERE ej.partida_id = @PartidaId
                          AND ej.time_id = @TimeId
                        ORDER BY ej.titular DESC, ej.numero_camisa";

            var dbPara = new DynamicParameters();
            dbPara.Add("@PartidaId", partidaId);
            dbPara.Add("@TimeId", timeId);

            try
            {
                var lista = await _context.GetAllAsync<EscalacaoJogoModel>(query, dbPara);
                return lista.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar escalação do time: {ex.Message}", ex);
            }
        }

        public async Task<List<EscalacaoJogoModel>> ListarPorPartida(long partidaId)
        {
            var query = SelectEscalacao + @"
                        WHERE ej.partida_id = @PartidaId
                        ORDER BY ej.time_id, ej.titular DESC, ej.numero_camisa";

            var dbPara = new DynamicParameters();
            dbPara.Add("@PartidaId", partidaId);

            try
            {
                var lista = await _context.GetAllAsync<EscalacaoJogoModel>(query, dbPara);
                return lista.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar escalação da partida: {ex.Message}", ex);
            }
        }

        public async Task RegistrarSaida(long id, short minuto)
        {
            await AtualizarMinuto("minuto_saida", id, minuto, "saída");
        }

        public async Task RegistrarEntrada(long id, short minuto)
        {
            await AtualizarMinuto("minuto_entrada", id, minuto, "entrada");
        }

        private async Task AtualizarMinuto(string coluna, long id, short minuto, string acao)
        {
            var query = $@"UPDATE escalacao_jogo SET {coluna} = @Minuto WHERE id = @Id";
            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);
            dbPara.Add("@Minuto", minuto);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao registrar {acao} na escalação: {ex.Message}", ex);
            }
        }

        public async Task<int> ContarJogosTitular(long jogadorId, long campeonatoId)
        {
            var query = @"
                        SELECT COUNT(*) AS JogosTitular
                        FROM escalacao_jogo ej
                        INNER JOIN partidas p ON p.id = ej.partida_id
                        WHERE ej.jogador_id = @JogadorId
                          AND p.campeonato_id = @CampeonatoId
                          AND ej.titular = TRUE";

            var dbPara = new DynamicParameters();
            dbPara.Add("@JogadorId", jogadorId);
            dbPara.Add("@CampeonatoId", campeonatoId);

            try
            {
                var total = await _context.ExecuteScalarAsync<long>(query, dbPara);
                return (int)total;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar jogos como titular: {ex.Message}", ex);
            }
        }

        public async Task Excluir(long id)
        {
            var query = "DELETE FROM escalacao_jogo WHERE id = @Id";
            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir jogador da escalação: {ex.Message}", ex);
            }
        }
    }
}
