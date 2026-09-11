namespace betsecrets.Modelos
{
    public class ClassificacaoModel
    {
        public long? Id { get; set; }
        public long? CampeonatoId { get; set; }
        public long? TimeId { get; set; }
        public int? Jogos { get; set; }
        public int? Vitorias { get; set; }
        public int? Empates { get; set; }
        public int? Derrotas { get; set; }
        public int? GolsPro { get; set; }
        public int? GolsContra { get; set; }
        public int? SaldoGols { get; set; }
        public int? Pontos { get; set; }
        public int? Posicao { get; set; }
        public string? Time { get; set; }
        public string? Escudo { get; set; }
    }
}
