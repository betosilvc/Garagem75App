using Garagem75.Shared.Dtos;

public class LoginResponseDto
{
    public bool Sucesso { get; set; }
    public string Token { get; set; } // Se você usar JWT
    public UsuarioDto Usuario { get; set; } // Os dados do usuário logado
}