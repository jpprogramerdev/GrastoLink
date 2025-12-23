using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGastroLink.Models {
    public class Mesa {
        public int Id { get; set; }
        public string Numero { get; set; }

        public string Status { get; set; }
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}