using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace APIGastroLink.DAO {
    public class DAOPagamento : IDAOPagamento {
        private readonly AppDbContext _context;
        private readonly IDAODatabase _database;

        public DAOPagamento(AppDbContext context, IDAODatabase database) {
            _context = context;
            _database = database;
        }

        public Task Delete(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }

        public async Task Insert(EntidadeDominio entidadeDominio) {
            string Procedure = "PROC_INSERIR_PAGAMENTO";

            var pagamento = (Pagamento)entidadeDominio;

            using (var conn = _database.GetConnection()) {
                using (var cmd = new SqlCommand(Procedure,conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VALOR_PAGO", pagamento.ValorPago);
                    cmd.Parameters.AddWithValue("@DESCONTO", pagamento.Desconto);
                    cmd.Parameters.AddWithValue("@DATA", pagamento.DataPagamento);
                    cmd.Parameters.AddWithValue("@FORMA_PAGAMENTO_ID", pagamento.FormaPagamentoId);
                    cmd.Parameters.AddWithValue("@PEDIDO_ID", pagamento.PedidoId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() => 
            await _context
            .Pagamentos
            .Include(p => p.FormaPagamento)
            .Include(p => p.Pedido)
            .ThenInclude(p => p.ItensPedido)
            .ThenInclude(i => i.Prato)
            .Include(p => p.Pedido)
            .ThenInclude(p => p.Mesa)
            .Include(p => p.Pedido)
            .ThenInclude(p => p.Usuario)
            .ToListAsync();

        public Task<EntidadeDominio> SelectById(int id) {
            throw new NotImplementedException();
        }

        public Task Update(EntidadeDominio entidadeDominio) {
            throw new NotImplementedException();
        }
    }
}
