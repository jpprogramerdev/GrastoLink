using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGastroLink.DAO {
    public class DAOPedido : IDAOPedido {
        private readonly AppDbContext _context;
        
        public DAOPedido(AppDbContext context) {
            _context = context;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task Insert(EntidadeDominio entidadeDominio) {
            _context.Pedidos.Add((Pedido)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<EntidadeDominio>> SelectAll() {
            throw new NotImplementedException();
        }

        public async Task<EntidadeDominio> SelectById(int id) => await _context.Pedidos.Include(p => p.ItensPedido).ThenInclude(i => i.Prato).Include(p => p.Mesa).Include(p => p.Usuario).SingleOrDefaultAsync(p => p.Id == id);

        public async Task Update(EntidadeDominio entidadeDominio) {
            var pedido = (Pedido)entidadeDominio;
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
