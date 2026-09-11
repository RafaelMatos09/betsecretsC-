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
    }
}
