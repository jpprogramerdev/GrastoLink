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

        public async Task Delete(EntidadeDominio entidadeDominio) => await _context.Database.ExecuteSqlRawAsync("EXEC PROC_EXCLUSAO_LOGICA_USUARIO @p0", ((Usuario)entidadeDominio).Id);

        public async Task Insert(EntidadeDominio entidadeDominio){
            _context.Usuarios.Add((Usuario)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.Usuarios.Include(u => u.TipoUsuario).ToListAsync();

        public async Task<EntidadeDominio> SelectById(int id) => await _context.Usuarios.Include(u => u.TipoUsuario).FirstOrDefaultAsync(u => u.Id == id);

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
