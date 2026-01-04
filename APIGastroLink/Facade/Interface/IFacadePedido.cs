using APIGastroLink.DTO;
using APIGastroLink.Models;

namespace APIGastroLink.Facade.Interface {
    public interface IFacadePedido{
        Task<PedidoResponseDTO> SalvarPedido(PedidoRequestDTO dto, int usuarioId);
        Task ExcluirPedido(int pedidoId);
        Task FinalizarPedido(int pedidoId);
    }
}
