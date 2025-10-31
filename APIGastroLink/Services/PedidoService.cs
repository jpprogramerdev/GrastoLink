using APIGastroLink.Models;
using APIGastroLink.Services.Interface;

namespace APIGastroLink.Services {
    public class PedidoService : IPedidoService {
        public decimal CalcularValorTotal(Pedido Pedido) {
            if (Pedido.ItensPedido == null || !Pedido.ItensPedido.Any()) {
                return 0m;
            }

            return Pedido.ItensPedido.Sum(i => i.Prato.Preco * i.Quantidade);
        }
    }
}
