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
                            data_nascimento AS dataNascimento,
                            cpf,
                            telefone,
                            foto,
                            pe_dominante AS peDominante,
                            posicao,
                            altura,
                            peso,
                            numero_preferido AS numeroPreferido
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
                req.Id = jogador.ToString();

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar jogador: {ex.Message}", ex);
            }
        }
    }
}
