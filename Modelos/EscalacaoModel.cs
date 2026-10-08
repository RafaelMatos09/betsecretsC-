namespace betsecrets.Modelos
{
    public class EscalacaoModel
    {
        public long Id { get; set; }
        public long PartidaId { get; set; }
        public long JogadorId { get; set; }
        public long TimeId { get; set; }
        public Boolean Titular { get; set; }
        public int NumeroCamisa { get; set; }
        public string? Posicao { get; set; }
        public int MinutoEntrada { get; set; }
        public int MinutoSaida { get; set; }
    }
}
