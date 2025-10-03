using APIGastroLink.Models;

namespace APIGastroLink.DTO {
    public class MesaDTO {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Status { get; set; }
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
