using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class EscalacaoRepository : IEscalacaoRepository
    {
        private readonly AppDbContext _context;

        public EscalacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EscalacaoModel> CadastrarEscalacao(EscalacaoModel req)
        {
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
                var dbPara = new DynamicParameters();

                dbPara.Add("@PartidaId", req.PartidaId);
                dbPara.Add("@JogadorId", req.JogadorId);
                dbPara.Add("@TimeId", req.TimeId);
                dbPara.Add("@Titular", req.Titular);
                dbPara.Add("@NumeroCamisa", req.NumeroCamisa);
                dbPara.Add("@Posicao", req.Posicao);
                dbPara.Add("@MinutoEntrada", req.MinutoEntrada);

                var id = await _context.ExecuteAsync(query, dbPara);

                req.Id = id;

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar escalacao: {ex.Message}", ex);
            }
        }
    }
}
