using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("PEDIDOS")]
    public class Pedido : EntidadeDominio {
        [Key]
        [Column("PED_ID")]
        public int Id { get; set; }
       
        [Required]
        [Column("PED_DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required] 
        [Column("PED_STATUS")]
        public string Status { get; set; }

        [Required]
        [Column("PED_VALOR_TOTAL")]
        public decimal ValorTotal { get; set; }

        // Foreign Keys
        [Column("PED_USU_ID")]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Column("PED_MSA_ID")]
        public int MesaId { get; set; }
        [ForeignKey("MesaId")]
        public Mesa Mesa { get; set; }
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
