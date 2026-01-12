using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.Data.SqlClient;

namespace APIGastroLink.DAO {
    public class DAOFormaPagamento : IDAOFormaPagamento {
        private readonly IDAODatabase _database;

        public DAOFormaPagamento(IDAODatabase database) {
            _database = database;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public Task Insert(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public Task<EntidadeDominio> SelectById(int id) {
            throw new NotImplementedException();
        }

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() {
            string sqlSelect = "SELECT * FROM FORMAS_PAGAMENTO";

            var listFormaPagamento = new List<EntidadeDominio>();

            using (var conn = _database.GetConnection()) {
                using (var cmd = new SqlCommand(sqlSelect, conn)) {
                    using(var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var formaPagamento = new FormaPagamento {
                                Id = reader.GetInt32(reader.GetOrdinal("FPG_ID")),
                                Forma = reader.GetString(reader.GetOrdinal("FPG_FORMA"))
                            };
                            listFormaPagamento.Add(formaPagamento);
                        }
                    }
                }
            }
            return listFormaPagamento;
        }
    }
}
