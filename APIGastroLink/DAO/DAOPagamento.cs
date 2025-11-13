using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGastroLink.DAO {
    public class DAOPagamento : IDAOPagamento {
        private readonly AppDbContext _context;

        public DAOPagamento(AppDbContext context) {
            _context = context;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task Insert(EntidadeDominio entidadeDominio) {
            _context.Pagamentos.Add((Pagamento)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => 
            await _context
            .Pagamentos
            .Include(p => p.FormaPagamento)
            .Include(p => p.Pedido)
            .ThenInclude(p => p.ItensPedido)
            .ThenInclude(i => i.Prato)
            .Include(p => p.Pedido)
            .ThenInclude(p => p.Mesa)
            .Include(p => p.Pedido)
            .ThenInclude(p => p.Usuario)
            .ToListAsync();

        public Task<EntidadeDominio> SelectById(int id) {
            throw new NotImplementedException();
        }

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
