using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class JogadorRepository : IJogadorRepository
    {
        private readonly AppDbContext _context;

        public JogadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JogadorModel> CadastraJogador(JogadorModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Apelido", req.Apelido);
            dbPara.Add("@DataNascimento", req.DataNascimento);
            dbPara.Add("@Cpf", req.Cpf);
            dbPara.Add("@Telefone", req.Telefone);
            dbPara.Add("@Foto", req.Foto);
            dbPara.Add("@PeDominante", req.PeDominante);
            dbPara.Add("@Posicao", req.Posicao);
            dbPara.Add("@Altura", req.Altura);
            dbPara.Add("@Peso", req.Peso);
            dbPara.Add("@NumeroPreferido", req.NumeroPreferido);

            var query = @"
                        INSERT INTO jogadores
                        (
                            nome,
                            apelido,
                            data_nascimento,
                            cpf,
                            telefone,
                            foto,
                            pe_dominante,
                            posicao,
                            altura,
                            peso,
                            numero_preferido
                        )
                        VALUES
                        (
                            @Nome,
                            @Apelido,
                            @DataNascimento,
                            @Cpf,
                            @Telefone,
                            @Foto,
                            @PeDominante,
                            @Posicao,
                            @Altura,
                            @Peso,
                            @NumeroPreferido
                        )
                        RETURNING id";            

            try
            {
                var jogador = await _context.ExecuteAsync(query, dbPara);
                req.Id = jogador;

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar jogador: {ex.Message}", ex);
            }
        }
       
        public async Task<JogadorModel?> ConsultaJogador(long id)
        {
            var query = @"
                        SELECT
                            id,
                            nome,
                            apelido,
                            data_nascimento AS DataNascimento,
                            foto,
                            pe_dominante AS PeDominante,
                            posicao,
                            altura,
                            peso,
                            numero_preferido AS NumeroPreferido
                        FROM jogadores
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            try
            {
                var jogador = await _context.GetAllAsync<JogadorModel>(
                    query,
                    dbPara
                );

                return jogador.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao consultar jogador: {ex.Message}",
                    ex
                );
            }
        }


        public async Task<List<JogadorModel>> BuscaJogadores(string nome)
        {
            var query = @"
                        SELECT
                            id,
                            nome,
                            apelido,
                            foto
                        FROM jogadores
                        WHERE nome ILIKE '%' || @Nome || '%'
                        ORDER BY nome
                        LIMIT 20";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Nome", nome);

            try
            {
                var jogadores = await _context.GetAllAsync<JogadorModel>(
                    query,
                    dbPara
                );

                return jogadores.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao buscar jogadores: {ex.Message}",
                    ex
                );
            }
        }


        public async Task AtualizaJogador(JogadorModel req)
        {
            var query = @"
                        UPDATE jogadores
                        SET
                            nome = @Nome,
                            apelido = @Apelido,
                            telefone = @Telefone,
                            foto = @Foto,
                            pe_dominante = @PeDominante,
                            posicao = @Posicao,
                            altura = @Altura,
                            peso = @Peso,
                            numero_preferido = @NumeroPreferido,
                            updated_at = NOW()
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", req.Id);
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Apelido", req.Apelido);
            dbPara.Add("@Telefone", req.Telefone);
            dbPara.Add("@Foto", req.Foto);
            dbPara.Add("@PeDominante", req.PeDominante);
            dbPara.Add("@Posicao", req.Posicao);
            dbPara.Add("@Altura", req.Altura);
            dbPara.Add("@Peso", req.Peso);
            dbPara.Add("@NumeroPreferido", req.NumeroPreferido);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao atualizar jogador: {ex.Message}",
                    ex
                );
            }
        }


        public async Task ExcluiJogador(long id)
        {
            var query = @"
                        DELETE FROM jogadores
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
                    $"Erro ao excluir jogador: {ex.Message}",
                    ex
                );
            }
        }

    }
}
