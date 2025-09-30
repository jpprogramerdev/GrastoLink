using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("LOG_PEDIDOS")]
    public class Log : EntidadeDominio {
        [Key]
        [Column("LPD_ID")]
        public int Id { get; set; }
        
        [Required]
        [Column("LPD_DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required]
        [Column("LPD_ACAO")]
        public string Acao { get; set; }

        //Foreign Key
        [Column("LPD_MSA_ID")]
        public int MesaId { get; set; }
        [ForeignKey("MesaId")]
        public Mesa Mesa{ get; set; }

        [Column("LPD_USU_ID")]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario{ get; set; }
    }
}
