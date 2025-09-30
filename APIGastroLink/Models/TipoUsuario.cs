using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("TIPOS_USUARIOS")]
    public class TipoUsuario : EntidadeDominio {
        [Key]
        [Column("TPU_ID")]
        public int Id { get; set; }

        [Required]
        [Column("TPU_TIPO")]
        public string Tipo { get; set; }

        // Relation
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
