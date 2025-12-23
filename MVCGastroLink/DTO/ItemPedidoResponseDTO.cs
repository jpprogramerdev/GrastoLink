namespace MVCGastroLink.DTO {
    public class ItemPedidoResponseDTO {
        public PratoDTO Prato { get; set; }
        public int Quantidade { get; set; }
        public string Status { get; set; }
    }
}