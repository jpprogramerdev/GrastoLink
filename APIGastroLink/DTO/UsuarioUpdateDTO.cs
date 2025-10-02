namespace APIGastroLink.DTO {
    public class UsuarioUpdateDTO {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public int TipoUsuarioId { get; set; }
    }
}
