using MVCGastroLink.Controllers;

namespace MVCGastroLink.DTO {
    public class LoginResponseDTO {
        public string Token { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }
}