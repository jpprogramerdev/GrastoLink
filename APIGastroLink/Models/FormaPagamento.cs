using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGastroLink.Models {
    [Table("FORMAS_PAGAMENTO")]
    public class FormaPagamento : EntidadeDominio {
        [Key]
        [Column("FPG_ID")]
        public int Id { get; set; }

        [Required]
        [Column("FPG_FORMA")]
        public string Forma { get; set; }

        //Relation
        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}
