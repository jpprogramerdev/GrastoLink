namespace APIGastroLink.Models {
    public class Pedido {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
        public decimal ValorTotal { get; set; }

        // Foreign Keys
        public Usuario Usuario { get; set; }
        public Mesa Mesa { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }
    }
}
