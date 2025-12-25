namespace APIGastroLink.DTO {
    public class ItemPedidoResponseDTO {
        public PratoDTO Prato { get; set; }
        public int Quantidade { get; set; }
        public string Status { get; set; }
        public string? Observacoes { get; set; }
    }
}
