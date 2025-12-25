using APIGastroLink.DTO;
using APIGastroLink.Models;

namespace APIGastroLink.Mapper.Interface {
    public interface IPedidoMapper {
        Pedido ToEntity(PedidoRequestDTO dto, int usuarioId);
        PedidoResponseDTO ToDTO(Pedido pedido);
    }
}
