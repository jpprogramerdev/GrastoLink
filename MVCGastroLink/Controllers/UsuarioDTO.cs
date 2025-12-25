namespace MVCGastroLink.Controllers {
    public class UsuarioDTO {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public bool Ativo { get; set; }
        public string TipoUsuario { get; set; }
    }
}