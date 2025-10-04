using APIGastroLink.Models;

namespace APIGastroLink.DAO.Interface {
    public interface IDAOMesa : IDAOGeneric{
        Task<Mesa> SelectByNumero(string numero);
    }
}
