namespace betsecrets.Modelos
{
    public class PartidaEventoModel
    {
        public long? Id { get; set; }
        public long? PartidaId { get; set; }
        public long? JogadorId { get; set; }
        public long? TimeId { get; set; }
        public int? Minuto { get; set; }
        public string? Tipo { get; set; }
        public string? Observacao { get; set; }
        public string? Jogador { get; set; }
        public string? Time { get; set; }
        public string? Foto { get; set; }
        public int? Gols { get; set; }
    }
}
