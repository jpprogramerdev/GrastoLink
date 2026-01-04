using APIGastroLink.Context;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace APIGastroLink.DAO {
    public class DAOPedido : IDAOPedido {
        private readonly AppDbContext _context;
        private readonly IDAODatabase _database;

        public DAOPedido(AppDbContext context, IDAODatabase database) {
            _context = context;
            _database = database;
        }

        public async Task Delete(EntidadeDominio entidadeDominio) {
            string deleteSql = "DELETE FROM ITENS_PEDIDO WHERE ITP_PED_ID = @IdPedido; DELETE FROM PEDIDOS WHERE PED_ID = @IdPedido;";
            
            var pedido = (Pedido)entidadeDominio;

            using (var conn = _database.GetConnection()) {
                using (var cmd = new SqlCommand(deleteSql, conn)) {
                    cmd.Parameters.AddWithValue("@IdPedido", pedido.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task Insert(EntidadeDominio entidadeDominio) {
            _context.Pedidos.Add((Pedido)entidadeDominio);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntidadeDominio>> SelectAll() {
            string selectSql = "SELECT * FROM VW_PEDIDOS_COMPLETO;";

            var listPedidos = new List<EntidadeDominio>();
            Dictionary<int, Pedido> pedidoDict = new Dictionary<int, Pedido>();

            using (var conn = _database.GetConnection()) {
                using (var cmd = new SqlCommand(selectSql, conn)) {
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) { 
                            var pedido = new Pedido();
                            var usuario = new Usuario();

                            pedido.Id = reader.GetInt32(reader.GetOrdinal("PED_ID"));

                            if (!pedidoDict.ContainsKey(pedido.Id)) {
                                pedido.DataHora = reader.GetDateTime(reader.GetOrdinal("PED_DATA_HORA"));
                                pedido.Status = reader.GetString(reader.GetOrdinal("PED_STATUS"));
                                pedido.Mesa = new Mesa {
                                    Id = reader.GetInt32(reader.GetOrdinal("MSA_ID")),
                                    Numero = reader.GetString(reader.GetOrdinal("MSA_NUMERO")),
                                    Status = (StatusMesa)reader.GetInt32(reader.GetOrdinal("MSA_STATUS"))
                                };
                                pedido.Usuario = new Usuario {
                                    Id = reader.GetInt32(reader.GetOrdinal("USU_ID")),
                                    Nome = reader.GetString(reader.GetOrdinal("USU_NOME"))
                                };

                                pedidoDict[pedido.Id] = pedido;
                            }

                            pedido = pedidoDict[pedido.Id];

                            var itemPedido = new ItemPedido {
                                Id = reader.GetInt32(reader.GetOrdinal("ITP_ID")),
                                Quantidade = reader.GetInt32(reader.GetOrdinal("ITP_QUANTIDADE")),
                                Status = reader.GetString(reader.GetOrdinal("ITP_STATUS")),
                                Observacoes = reader.IsDBNull(reader.GetOrdinal("ITP_OBSERVACOES")) ? null : reader.GetString(reader.GetOrdinal("ITP_OBSERVACOES")),
                                Prato = new Prato {
                                    Id = reader.GetInt32(reader.GetOrdinal("PRA_ID")),
                                    Nome = reader.GetString(reader.GetOrdinal("PRA_NOME")),
                                    Descricao = reader.GetString(reader.GetOrdinal("PRA_DESCRICAO")),
                                    Preco = reader.GetDecimal(reader.GetOrdinal("PRA_PRECO"))
                                } 
                            };

                            pedido.ItensPedido.Add(itemPedido);

                            if (!listPedidos.Cast<Pedido>().Any(p => p.Id == pedido.Id)) {
                                listPedidos.Add(pedido);
                            }
                        }
                    }
                }
            }

            return listPedidos;
        }


        public async Task<EntidadeDominio> SelectById(int id) => await _context.Pedidos
            .Include(p => p.ItensPedido)
                .ThenInclude(i => i.Prato)
            .Include(p => p.Mesa)
            .Include(p => p.Usuario)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);

        public async Task Update(EntidadeDominio entidadeDominio) {
            string updateSQL = "UPDATE PEDIDOS SET PED_STATUS = 'FINALIZADO' WHERE PED_ID = @Id;";

            var pedido = (Pedido)entidadeDominio;

            using (var conn = _database.GetConnection()) {
                using (var cmd = new SqlCommand(updateSQL, conn)) {
                    cmd.Parameters.AddWithValue("@Id", pedido.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
