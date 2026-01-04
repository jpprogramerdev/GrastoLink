using Microsoft.Data.SqlClient;

namespace APIGastroLink.DAO.Interface {
    public interface IDAODatabase {
        public SqlConnection GetConnection();
        public void CloseConnection(SqlConnection connection);
    }
}
