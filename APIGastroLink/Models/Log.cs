namespace APIGastroLink.Models {
    public class Log {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Acao { get; set; }

        //Foreign Key
        public Mesa Mesa{ get; set; }
        public Usuario Usuario{ get; set; }
    }
}
