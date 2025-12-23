using APIGastroLink.Models;

namespace APIGastroLink.DTO {
    public class LoginResponse {
        public string Token { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }
}
