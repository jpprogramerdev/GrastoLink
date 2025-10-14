using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;

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

        public Task<EntidadeDominio> SelectById(int id) {
            throw new NotImplementedException();
        }

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
