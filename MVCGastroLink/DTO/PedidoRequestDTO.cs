namespace MVCGastroLink.DTO {
    public class PedidoRequestDTO {
        public int MesaId { get; set; }
        public string? Observacoes { get; set; }

        public List<ItemPedidoCreateDTO> ItensPedido { get; set; }
    }
}
