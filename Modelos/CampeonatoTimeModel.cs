namespace betsecrets.Modelos
{
    public class CampeonatoTimeModel
    {
        public long? Id { get; set; }
        public long? CampeonatoId { get; set; }
        public long? TimeId { get; set; }
        public string? Grupo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Nome { get; set; }
        public string? Escudo { get; set; }
    }
}
