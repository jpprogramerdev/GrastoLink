using APIGastroLink.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace APIGastroLink.DAO {
    public class DAOSqlServer : IDAODatabase {
        private readonly string _connectionString;

        public DAOSqlServer(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void CloseConnection(SqlConnection connection) {
            if(connection != null && connection.State == System.Data.ConnectionState.Open) {
                connection.Close();
            }
        }

        public SqlConnection GetConnection() {
            var conn =  new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
