namespace APIGastroLink.DTO {
    public class PedidoCreateDTO {
        public int UsuarioId { get; set; }
        public int MesaId { get; set; }

        public List<ItemPedidoCreateDTO> ItensPedido { get; set; }
    }
}
