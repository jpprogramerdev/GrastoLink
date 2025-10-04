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

        public async Task Insert(EntidadeDominio entidadeDominio) {
            _context.Add((CategoriaPrato)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.CategoriasPratos.Include(p => p.Pratos).ToListAsync();

        public async Task<EntidadeDominio> SelectById(int id) => await _context.CategoriasPratos.Include(p => p.Pratos).SingleOrDefaultAsync(c => c.Id == id);

        public async Task Update(EntidadeDominio entidadeDominio) {
            var categoriaPrato = (CategoriaPrato)(entidadeDominio);

            var categoriaPratoExistente = await _context.CategoriasPratos.FindAsync(categoriaPrato.Id);

            if(categoriaPratoExistente == null) {
                throw new KeyNotFoundException();
            }

            categoriaPratoExistente.Categoria = categoriaPrato.Categoria;

            await _context.SaveChangesAsync();
        }
    }
}
