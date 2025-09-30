namespace APIGastroLink.Models {
    public class ItemPedido {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public string? Observacoes { get; set; }
        public string Status { get; set; }

        // Foreign Keys
        public Prato Prato { get; set; }
        public Pedido Pedido { get; set; }
    }
}
