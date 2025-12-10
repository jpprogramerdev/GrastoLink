using APIGastroLink.Models;

namespace APIGastroLink.DAO.Interface {
    public interface IDAOUsuario: IDAOGeneric {
        public Task<EntidadeDominio> Authenticate(string cpf, string senha);
    }
}
