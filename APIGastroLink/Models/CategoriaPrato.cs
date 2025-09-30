using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("CATEGORIAS_PRATOS")]
    public class CategoriaPrato : EntidadeDominio {
        [Key]
        [Column("CTP_ID")]
        public int Id { get; set; }

        [Required]
        [Column("CTP_CATEGORIA")]
        public string Categoria { get; set; }

        // Relation
        public ICollection<Prato> Pratos { get; set; } = new List<Prato>();
    }
}
