using APIGastroLink.Models;

namespace APIGastroLink.DTO {
    public class PagamentoDTO {
        public int Id { get; set; }
        public decimal ValorPago { get; set; }
        public decimal Desconto { get; set; }
        public DateTime DataPagamento { get; set; }
        public FormaPagamentoDTO FormaPagamento { get; set; }
        public PedidoResponseDTO Pedido { get; set; }
    }
}
