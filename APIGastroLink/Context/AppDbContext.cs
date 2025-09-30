using APIGastroLink.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGastroLink.Context {
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CategoriaPrato> CategoriasPratos { get; set; }
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Prato> Pratos { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
