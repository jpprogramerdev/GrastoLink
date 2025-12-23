using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGastroLink.Models {
    [Table("USUARIOS")]
    public class Usuario {
        [Key]
        [Column("USU_ID")]
        public int Id { get; set; }

        [Required]
        [Column("USU_NOME")]
        public string Nome { get; set; }

        [Required]
        [Column("USU_CPF")]
        public string CPF { get; set; }

        [Required]
        [Column("USU_SENHA")]
        public string Senha { get; set; }

        [Required]
        [Column("USU_ATIVO")]
        public bool Ativo { get; set; }

        // Foreign Key
        [Column("USU_TPU_ID")]
        public int TipoUsuarioId { get; set; }
        [ForeignKey("TipoUsuarioId")]
        public TipoUsuario TipoUsuario { get; set; }
    }
}
