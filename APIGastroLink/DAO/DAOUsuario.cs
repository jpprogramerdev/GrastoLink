using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APIGastroLink.DAO {
    public class DAOUsuario : IDAOUsuario {
        private readonly AppDbContext _context;

        public DAOUsuario(AppDbContext context) {
            _context = context;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task Insert(EntidadeDominio entidadeDominio){
            _context.Usuarios.Add((Usuario)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.Usuarios.Include(u => u.TipoUsuario).ToListAsync();

        public Task<EntidadeDominio> SelectById(int id) {
            throw new NotImplementedException();
        }

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
