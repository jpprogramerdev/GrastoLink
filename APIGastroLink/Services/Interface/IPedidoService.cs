using APIGastroLink.Models;

namespace APIGastroLink.Services.Interface {
    public interface IPedidoService {
        decimal CalcularValorTotal(Pedido Pedido);
    }
}
