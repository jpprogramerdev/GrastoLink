namespace MVCGastroLink.DTO {
    public class CategoriaPratoDTO {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public List<PratoDTO> Pratos { get; set; } = new List<PratoDTO>();
    }
}