namespace MVCGastroLink.DTO {
    public class PagamentoRequestDTO {
        public decimal ValorPago { get; set; }
        public decimal Desconto { get; set; }
        public int FormaPagamentoId { get; set; }

        public int PedidoId { get; set; }
    }
}
