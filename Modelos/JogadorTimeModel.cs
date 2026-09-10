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
    }
}
