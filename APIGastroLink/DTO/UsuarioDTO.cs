using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.DTO {
    public class UsuarioDTO {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public string Tipo { get; set; }
    }
}
