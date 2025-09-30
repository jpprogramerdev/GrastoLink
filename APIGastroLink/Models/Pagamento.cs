namespace APIGastroLink.Models {
    public class Pagamento {
        public int Id { get; set; }
        public decimal ValorPago { get; set; }
        public decimal Desconto { get; set; }
        public DateTime DataPagamento { get; set; }

        // Foreign Keys
        public FormaPagamento FormaPagamento { get; set; }
        public Usuario Usuario { get; set; }
        public Pedido Pedido { get; set; }
    }
}
