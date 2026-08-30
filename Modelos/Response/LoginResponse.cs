namespace betsecrets.Modelos.Response
{
    public class LoginResponse
    {
        public string? Token { get; set; } = string.Empty;

        public UsuarioModel? Usuario { get; set; } = new();
    }
}
