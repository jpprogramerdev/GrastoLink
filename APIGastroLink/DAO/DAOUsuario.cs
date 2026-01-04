using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APIGastroLink.DAO {
    public class DAOUsuario : IDAOUsuario {
        private readonly AppDbContext _context;
        private readonly IDAODatabase _database;

        public DAOUsuario(IDAODatabase database, AppDbContext context) {
            _context = context;
            _database = database;
        }

        public async Task Delete(EntidadeDominio entidadeDominio) => await _context.Database.ExecuteSqlRawAsync("EXEC PROC_EXCLUSAO_LOGICA_USUARIO @p0", ((Usuario)entidadeDominio).Id);

        public async Task Insert(EntidadeDominio entidadeDominio){
            _context.Usuarios.Add((Usuario)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => await _context.Usuarios.Include(u => u.TipoUsuario).ToListAsync();

        public async Task<EntidadeDominio> SelectById(int id) => await _context.Usuarios.Include(u => u.TipoUsuario).FirstOrDefaultAsync(u => u.Id == id);

        public async Task Update(EntidadeDominio entidadeDominio) {
            var usuario = (Usuario)entidadeDominio;

            var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);

            if(usuarioExistente == null) {
                throw new KeyNotFoundException();
            }

            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.CPF = usuario.CPF;
            usuarioExistente.Senha = usuario.Senha;
            usuarioExistente.Ativo = usuario.Ativo;
            usuarioExistente.TipoUsuarioId = usuario.TipoUsuarioId;

            _context.Usuarios.Update(usuarioExistente);
            await _context.SaveChangesAsync();
        }

        public async Task<EntidadeDominio> Authenticate(string cpf, string senha) {
            string sql = "SELECT * FROM USUARIOS JOIN TIPOS_USUARIOS ON TPU_ID = USU_TPU_ID WHERE USU_CPF = @CPF AND USU_SENHA = @SENHA";

            var usuario = new Usuario();

            using (var conn = _database.GetConnection()) {
                using (var cmd = new SqlCommand(sql, conn)) {
                    cmd.Parameters.AddWithValue("@CPF", cpf);
                    cmd.Parameters.AddWithValue("@SENHA", senha);
                    using (var reader = await cmd.ExecuteReaderAsync()) {
                        if (await reader.ReadAsync()) {
                            usuario.Id = reader.GetInt32(reader.GetOrdinal("USU_ID"));
                            usuario.Nome = reader.GetString(reader.GetOrdinal("USU_NOME"));
                            usuario.CPF = reader.GetString(reader.GetOrdinal("USU_CPF"));
                            usuario.Ativo = reader.GetBoolean(reader.GetOrdinal("USU_ATIVO"));
                            usuario.TipoUsuario = new TipoUsuario {
                                Id = reader.GetInt32(reader.GetOrdinal("USU_TPU_ID")),
                                Tipo = reader.GetString(reader.GetOrdinal("TPU_TIPO"))
                            };
                            usuario.TipoUsuarioId = reader.GetInt32(reader.GetOrdinal("USU_TPU_ID"));
                            return usuario;
                        } else {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
