using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGastroLink.DAO {
    public class DAOPrato : IDAOPrato {
        private readonly AppDbContext _context;

        public DAOPrato(AppDbContext context) {
            _context = context;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public Task Insert(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.Pratos.Include(p => p.CategoriaPrato).ToListAsync();


        public async Task<EntidadeDominio> SelectById(int id) => await _context.Pratos.Include(p => p.CategoriaPrato).SingleOrDefaultAsync(p => p.Id == id);

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
