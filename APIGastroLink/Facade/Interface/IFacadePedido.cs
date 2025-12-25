using APIGastroLink.DTO;

namespace APIGastroLink.Facade.Interface {
    public interface IFacadePedido{
        Task<PedidoResponseDTO> SalvarPedido(PedidoRequestDTO dto, int usuarioId);
    }
}
