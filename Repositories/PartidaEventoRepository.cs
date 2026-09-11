using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class PartidaEventoRepository : IPartidaEventoRepository
    {
        private readonly AppDbContext _context;

        public PartidaEventoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PartidaEventoModel> CadastrarPartidaEvento(PartidaEventoModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@PartidaId", req.PartidaId);
            dbPara.Add("@JogadorId", req.JogadorId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@Minuto", req.Minuto);
            dbPara.Add("@Tipo", req.Tipo);
            dbPara.Add("@Observacao", req.Observacao);

            var query = @"
                        INSERT INTO partida_eventos
                        (
                            partida_id,
                            jogador_id,
                            time_id,
                            minuto,
                            tipo,
                            observacao
                        )
                        VALUES
                        (
                            @PartidaId,
                            @JogadorId,
                            @TimeId,
                            @Minuto,
                            @Tipo,
                            @Observacao
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
                throw new Exception($"Erro ao cadastrar evento da partida: {ex.Message}", ex);
            }
        }
    }
}
