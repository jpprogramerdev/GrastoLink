using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APIGastroLink.DAO {
    public class DAOMesa : IDAOMesa {
        private readonly AppDbContext _context;

        public DAOMesa(AppDbContext context) {
            _context = context;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task Insert(EntidadeDominio entidadeDominio) {
            _context.Mesas.Add((Mesa)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.Mesas.ToListAsync();

        public async Task<EntidadeDominio> SelectById(int id) => await _context.Mesas.SingleOrDefaultAsync(m => m.Id == id);

        public async Task<Mesa> SelectByNumero(string numero) => await _context.Mesas.SingleOrDefaultAsync(m => m.Numero == numero);

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
