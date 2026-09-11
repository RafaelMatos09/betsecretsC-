namespace betsecrets.Modelos
{
    public class JogadorModel
    {
        public long? Id { get; set; }
        public string? Nome { get; set; }
        public string? Apelido { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Foto { get; set; }
        public string? PeDominante { get; set; }
        public string? Posicao { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public int NumeroPreferido { get; set; }
        public string? CreatedAt { get; set; }
    }
}
