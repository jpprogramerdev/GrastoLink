using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("PAGAMENTOS")]
    public class Pagamento : EntidadeDominio {
        [Key]
        [Column("PAG_ID")]
        public int Id { get; set; }

        [Required]
        [Column("PAG_VALOR_PAGO")]
        public decimal ValorPago { get; set; }

        [Required]
        [Column("PAG_DESCONTO")]
        public decimal Desconto { get; set; }

        [Required]
        [Column("PAG_DATA_PAGAMENTO")]
        public DateTime DataPagamento { get; set; }

        // Foreign Keys
        [Column("PAG_FPG_ID")]
        public int FormaPagamentoId { get; set; }
        [ForeignKey("FormaPagamentoId")]
        public FormaPagamento FormaPagamento { get; set; }

        [Column("PAG_PED_ID")]
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }
    }
}
