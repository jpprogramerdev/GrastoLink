using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGastroLink.Models {
    public class Prato {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public double TempoMedioPreparo { get; set; }
        public bool Disponivel { get; set; }
        public CategoriaPrato CategoriaPrato { get; set; }
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}