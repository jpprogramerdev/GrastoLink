using APIGastroLink.Models;

namespace APIGastroLink.DAO.Interface {
    public interface IDAOPrato : IDAOGeneric{
        public Task<IEnumerable<Prato>> SelectAllDisponivel();
    }
}
