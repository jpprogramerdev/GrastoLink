using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using APIGastroLink.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/pedido")]
    [Authorize]
    public class PedidoController : ControllerBase {
        private IDAOPedido _daoPedido;
        private IPedidoService _pedidoService;

        public PedidoController(IDAOPedido daoPedido, IPedidoService pedidoService) {
            _daoPedido = daoPedido;
            _pedidoService = pedidoService;
        }

        //GET api-gastrolink/pedido/todos-aberto
        [HttpGet("todos-aberto")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PedidoResponseDTO>>> GetPedidosAbertos() {
            try {
                var pedidos = (await _daoPedido.SelectAll()).Cast<Pedido>().Where(p => p.Status != "FINALIZADO" && p.Status != "CANCELADO").ToList();

                if (pedidos == null || pedidos.Count == 0) {
                    return Ok("Nenhum pedido aberto encontrado.");
                }

                var pedidosResponse = pedidos.Select(p => new PedidoResponseDTO {
                    Id = p.Id,
                    DataHora = p.DataHora,
                    Status = p.Status,
                    Mesa = new MesaDTO {
                        Id = p.Mesa.Id,
                        Numero = p.Mesa.Numero,
                        Status = p.Mesa.Status.ToString()
                    },
                    UsuarioId = p.UsuarioId,
                    ValorTotal = _pedidoService.CalcularValorTotal(p),
                    Itens = p.ItensPedido.Select(i => new ItemPedidoResponseDTO {
                        Prato = new PratoDTO {
                            Nome = i.Prato.Nome,
                        },
                        Quantidade = i.Quantidade,
                        Status = i.Status
                    }).ToList()
                }).ToList();

                return Ok(pedidosResponse);
            } catch (Exception ex) {
                return BadRequest($"Erro ao recuperar pedidos: {ex.Message}");
            }
        }

        //POST api-gastrolink/pedido
        [HttpPost]
        public async Task<ActionResult<PedidoResponseDTO>> PostPedido([FromBody] PedidoCreateDTO PedidoCreateDTO) {
            if (PedidoCreateDTO == null) {
                return BadRequest("Dados invalidos.");
            }

            var pedido = new Pedido {
                UsuarioId = PedidoCreateDTO.UsuarioId,
                MesaId = PedidoCreateDTO.MesaId,
                DataHora = DateTime.Now,
                Status = "RECEBIDO",
                ValorTotal = 0,
                ItensPedido = PedidoCreateDTO.ItensPedido.Select(ip => new ItemPedido {
                    PratoId = ip.PratoId,
                    Quantidade = ip.Quantidade,
                    Status = "RECEBIDO"
                }).ToList()
            };

            pedido.ValorTotal = _pedidoService.CalcularValorTotal(pedido);


            try {
                await _daoPedido.Insert(pedido);

                var pedidoResponse = new PedidoResponseDTO {
                    Id = pedido.Id,
                    DataHora = pedido.DataHora,
                    Status = pedido.Status,
                    Mesa = new MesaDTO {
                        Id = pedido.Mesa.Id,
                        Numero = pedido.Mesa.Numero,
                        Status = pedido.Mesa.Status.ToString()
                    },
                    UsuarioId = pedido.UsuarioId,
                    ValorTotal = _pedidoService.CalcularValorTotal(pedido),
                    Itens = pedido.ItensPedido.Select(i => new ItemPedidoResponseDTO {
                        Prato = new PratoDTO { 
                            Id = i.Prato.Id,
                            Nome = i.Prato.Nome,
                        },
                        Quantidade = i.Quantidade,
                        Status = i.Status
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetPedido), new { pedidoId = pedido.Id }, pedidoResponse);


            } catch (Exception ex) {
                return BadRequest($"Falha ao salvar o pedido: {ex.Message}");
            }
        }

        //GET api-gastrolink/pedido/{pedidoId}
        [HttpGet("{pedidoId}")]
        public async Task<ActionResult<PedidoResponseDTO>> GetPedido(int pedidoId) {
            var pedido = (await _daoPedido.SelectById(pedidoId)) as Pedido;
            if (pedido == null) {
                return NotFound("Pedido nao encontrado.");
            }

            var pedidoResponse = new PedidoResponseDTO {
                Id = pedido.Id,
                DataHora = pedido.DataHora,
                Status = pedido.Status,
                Mesa = new MesaDTO {
                    Id = pedido.Mesa.Id,
                    Numero = pedido.Mesa.Numero,
                    Status = pedido.Mesa.Status.ToString()
                },
                UsuarioId = pedido.UsuarioId,
                ValorTotal = _pedidoService.CalcularValorTotal(pedido),
                Itens = pedido.ItensPedido.Select(i => new ItemPedidoResponseDTO {
                    Prato = new PratoDTO {
                        Id = i.Prato.Id,
                        Nome = i.Prato.Nome,
                    },
                    Quantidade = i.Quantidade,
                    Status = i.Status
                }).ToList()
            };

            return Ok(pedidoResponse);
        }

        //POST api-gastrolink/pedido/{pedidoId}/itens
        [HttpPost("{pedidoId}/itens")]
        public async Task<ActionResult<ItemPedidoResponseDTO>> AdicionarItem(int pedidoId, [FromBody] ItemPedidoCreateDTO ItemPedidoCreateDTO) {
            var pedido = (await _daoPedido.SelectById(pedidoId)) as Pedido;

            if (pedido == null) {
                return NotFound("Pedido nao encontrado.");
            }

            var novoItem = new ItemPedido {
                PratoId = ItemPedidoCreateDTO.PratoId,
                Quantidade = ItemPedidoCreateDTO.Quantidade,
                Observacoes = ItemPedidoCreateDTO.Observacoes,
                Status = "RECEBIDO",
                PedidoId = pedidoId
            };

            pedido.ItensPedido.Add(novoItem);
            try {
                await _daoPedido.Update(pedido);
                var itemResponse = new ItemPedidoResponseDTO {
                    Prato = new PratoDTO {
                        Id = novoItem.Prato.Id,
                        Nome = novoItem.Prato.Nome,
                    },
                    Quantidade = novoItem.Quantidade,
                    Status = novoItem.Status
                };
                return Created("Item adicionado com sucesso", itemResponse);
            } catch (Exception ex) {
                return BadRequest($"Falha ao adicionar o item: {ex.Message}");
            }
        }
    }
}
