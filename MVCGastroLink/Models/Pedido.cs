using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGastroLink.Models {
    public class Pedido {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
        public decimal ValorTotal { get; set; }
        public Usuario Usuario { get; set; }
        public Mesa Mesa { get; set; }
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
