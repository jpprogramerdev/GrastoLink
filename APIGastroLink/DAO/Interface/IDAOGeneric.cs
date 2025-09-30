using APIGastroLink.Models;

namespace APIGastroLink.DAO.Interface {
    public interface IDAOGeneric {
        public Task<IEnumerable<EntidadeDominio>> SelectAll();
        public Task Insert(EntidadeDominio entidadeDominio);
        public Task Update(EntidadeDominio entidadeDominio);
        public Task Delete(EntidadeDominio entidadeDominio);
        public Task<EntidadeDominio> SelectById(int id);
    }
}
