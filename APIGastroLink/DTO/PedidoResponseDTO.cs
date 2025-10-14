namespace APIGastroLink.DTO {
    public class PedidoResponseDTO {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
        public int MesaId { get; set; }
        public int UsuarioId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<ItemPedidoResponseDTO> Itens { get; set; }
    }
}
