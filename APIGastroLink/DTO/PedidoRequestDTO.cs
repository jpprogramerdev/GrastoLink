namespace APIGastroLink.DTO {
    public class PedidoRequestDTO {
        public int MesaId { get; set; }

        public List<ItemPedidoCreateDTO> ItensPedido { get; set; }
    }
}
