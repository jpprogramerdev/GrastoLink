using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;

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
