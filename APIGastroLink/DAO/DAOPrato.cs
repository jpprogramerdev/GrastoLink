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

        public async Task Insert(EntidadeDominio entidadeDominio) {
            _context.Pratos.Add((Prato)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.Pratos.Include(p => p.CategoriaPrato).ToListAsync();

        public async Task<IEnumerable<Prato>> SelectAllDisponivel() => await _context.Pratos.Include(p => p.CategoriaPrato).Where(p => p.Disponivel).ToListAsync();

        public async Task<EntidadeDominio> SelectById(int id) => await _context.Pratos.Include(p => p.CategoriaPrato).SingleOrDefaultAsync(p => p.Id == id);

        public async Task Update(EntidadeDominio entidadeDominio) {
            var prato = (Prato)entidadeDominio;

            var pratoExistente = await _context.Pratos.FirstOrDefaultAsync(p => p.Id == prato.Id);

            if (pratoExistente == null) {
                throw new KeyNotFoundException();
            }

            pratoExistente.Id = prato.Id;
            pratoExistente.Descricao = prato.Descricao;
            pratoExistente.Nome = prato.Nome;
            pratoExistente.Disponivel = prato.Disponivel;
            pratoExistente.CategoriaPratoId = prato.CategoriaPratoId;
            pratoExistente.Preco = prato.Preco;
            pratoExistente.TempoMedioPreparo = prato.TempoMedioPreparo;

            _context.Pratos.Update(pratoExistente);
            await _context.SaveChangesAsync();
        }
    }
}
