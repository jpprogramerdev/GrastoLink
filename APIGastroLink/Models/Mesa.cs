using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("MESAS")]
    public class Mesa : EntidadeDominio {
        [Key]
        [Column("MSA_ID")]
        public int Id { get; set; }

        [Required]
        [Column("MSA_NUMERO")]
        public string Numero { get; set; }

        [Required]
        [Column("MSA_STATUS")]
        public StatusMesa Status { get; set; }

        //Relation
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    }
}
