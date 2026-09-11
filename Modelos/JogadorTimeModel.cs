namespace betsecrets.Modelos
{
    public class JogadorTimeModel
    {
        public int Id { get; set; }
        public long JogadorId { get; set; }
        public long TimeId { get; set; }
        public int NumeroCamisa { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public Boolean Ativo { get; set; }
        public string? Nome { get; set; }
        public string? Apelido { get; set; }
        public string? Foto { get; set; }
        public string? Posicao { get; set; }
        public string? TimeNome { get; set; }
    }
}
