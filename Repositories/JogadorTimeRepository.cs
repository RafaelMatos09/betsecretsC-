using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class JogadorTimeRepository : IJogadorTimeRepository
    {
        private readonly AppDbContext _context;

        public JogadorTimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JogadorTimeModel> CadastrarJogadorTime(JogadorTimeModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@JogadorId", req.JogadorId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@NumeroCamisa", req.NumeroCamisa);                       
            
            var query = @"
                        INSERT INTO jogadores_times
                        (
                            jogador_id,
                            time_id,
                            numero_camisa,
                            data_inicio                        
                        )
                        VALUES
                        (
                            @JogadorId,
                            @TimeId,
                            @NumeroCamisa,
                            NOW()                         
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
                throw new Exception($"Erro ao cadastrar jogador no time: {ex.Message}");
            }
        }
    }
}
