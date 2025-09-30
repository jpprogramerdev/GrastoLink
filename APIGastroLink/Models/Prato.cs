namespace APIGastroLink.Models {
    public class Prato {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public double TempoMedioPreparo { get; set; }
        public bool Disponivel { get; set; }

        // Foreign Key
        public CategoriaPrato CategoriaPrato { get; set; }
    }
}
