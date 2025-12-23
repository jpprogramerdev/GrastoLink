using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGastroLink.Models {
    public class CategoriaPrato {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public ICollection<Prato> Pratos { get; set; } = new List<Prato>();
    }
}