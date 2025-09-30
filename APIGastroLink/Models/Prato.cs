using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("PRATOS")]
    public class Prato {
        [Key]
        [Column("PRA_ID")]
        public int Id { get; set; }

        [Required]
        [Column("PRA_NOME")]
        public string Nome { get; set; }

        [Required]
        [Column("PRA_DESCRICAO")]
        public string Descricao { get; set; }

        [Required]
        [Column("PRA_PRECO")]
        public decimal Preco { get; set; }

        [Required]
        [Column("PRA_TEMPO_MEDIO_PREPARO")]
        public double TempoMedioPreparo { get; set; }

        [Required]
        [Column("PRA_DISPONIVEL")]
        public bool Disponivel { get; set; }

        // Foreign Key
        [Column("PRA_CTP_ID")]
        public int CategoriaPratoId { get; set; }
        [ForeignKey("CategoriaPratoId")]
        public CategoriaPrato CategoriaPrato { get; set; }
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
