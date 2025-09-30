using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("ITENS_PEDIDO")]
    public class ItemPedido : EntidadeDominio {
        [Key]
        [Column("ITP_ID")]
        public int Id { get; set; }

        [Required]
        [Column("ITP_QUANTIDADE")]
        public int Quantidade { get; set; }

        [Column("ITP_OBSERVACOES")]
        public string? Observacoes { get; set; }

        [Required]
        [Column("ITP_STATUS")]
        public string Status { get; set; }

        // Foreign Keys
        [Column("ITP_PRA_ID")]
        public int PratoId { get; set; }
        [ForeignKey("PratoId")]
        public Prato Prato { get; set; }

        [Column("ITP_PED_ID")]
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }
    }
}
