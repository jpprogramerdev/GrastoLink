using APIGastroLink.Models;

namespace APIGastroLink.DTO {
    public class PedidoRequestDTO : EntidadeDominio {
        public int MesaId { get; set; }

        public List<ItemPedidoCreateDTO> ItensPedido { get; set; }
    }
}
