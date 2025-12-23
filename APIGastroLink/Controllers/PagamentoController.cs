using APIGastroLink.DAO;
using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using APIGastroLink.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/pagamento")]
    public class PagamentoController : ControllerBase {
        private readonly IDAOPagamento _daoPagamento;
        private IPedidoService _pedidoService;

        public PagamentoController(IDAOPagamento daoPagamento, IPedidoService pedidoService) {
            _daoPagamento = daoPagamento;
            _pedidoService = pedidoService;
        }


        //POST api-gastrolink/pagamento
        [HttpPost]
        public async Task<ActionResult<Pagamento>> PostPagamento([FromBody]PagamentoCreateDTO PagamentoCreateDTO) {
            if(PagamentoCreateDTO == null) {
                return BadRequest("Dados invalidos.");
            }

            var pagamento = new Pagamento {
                ValorPago = PagamentoCreateDTO.ValorPago,
                Desconto = PagamentoCreateDTO.Desconto,
                DataPagamento = DateTime.Now,
                FormaPagamentoId = PagamentoCreateDTO.FormaPagamentoId,
                PedidoId = PagamentoCreateDTO.PedidoId
            };

            try {
                await _daoPagamento.Insert(pagamento);

                return Created("Pagamento processado com sucesso", pagamento);
            } catch (Exception ex) {
                return BadRequest($"Falha ao processar o pagamento: {ex.Message}");
            }
        }

        //GET api-gastrolink/pagamento/todos
        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetAllPagamentos() {
            try {
                var pagamentos = (await _daoPagamento.SelectAll()).Cast<Pagamento>().ToList();
                var pagamentosDTO = new List<PagamentoDTO>();


                foreach (var p in pagamentos) {
                    pagamentosDTO.Add(new PagamentoDTO {
                        Id = p.Id,
                        ValorPago = p.ValorPago,
                        Desconto = p.Desconto,
                        DataPagamento = p.DataPagamento,
                        FormaPagamento = new FormaPagamentoDTO { 
                            Id = p.FormaPagamento.Id,
                            Forma = p.FormaPagamento.Forma
                        },
                        Pedido = new PedidoResponseDTO {
                            Id = p.Pedido.Id,
                            DataHora = p.Pedido.DataHora,
                            Status = p.Pedido.Status,
                            Mesa = new MesaDTO {
                                Id = p.Pedido.Mesa.Id,
                                Numero = p.Pedido.Mesa.Numero,
                                Status = p.Pedido.Mesa.Status.ToString()
                            },
                            UsuarioId = p.Pedido.UsuarioId,
                            ValorTotal = _pedidoService.CalcularValorTotal(p.Pedido),
                            Itens = p.Pedido.ItensPedido.Select(i => new ItemPedidoResponseDTO {
                                Prato = new PratoDTO { 
                                    Id = i.Prato.Id,
                                    Nome = i.Prato.Nome,
                                },
                                Quantidade = i.Quantidade,
                                Status = i.Status
                            }).ToList()
                        }
                    });
                }
                return Ok(pagamentosDTO);
            } catch (Exception ex) {
                return BadRequest($"Falha ao obter os pagamentos: {ex.Message}");
            }
        }
    }
}
