namespace betsecrets.Modelos
{
    public class PracaModel
    {
        public long? Id { get; set; }
        public long? BairroId { get; set; }
        public string? Nome { get; set; }
        public string? Endereco { get; set; }
        public string? Tipo { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Observacoes { get; set; }
        public string? BairroNome { get; set; }
        public int? JogosMarcados { get; set; }
    }
}
