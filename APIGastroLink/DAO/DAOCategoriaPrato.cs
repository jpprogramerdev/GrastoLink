using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGastroLink.DAO {
    public class DAOCategoriaPrato : IDAOCategoriaPrato {
        private readonly AppDbContext _context;

        public DAOCategoriaPrato(AppDbContext context) {
            _context = context;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public Task Insert(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.CategoriasPratos.ToListAsync();

        public Task<EntidadeDominio> SelectById(int id) {
            throw new NotImplementedException();
        }

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
