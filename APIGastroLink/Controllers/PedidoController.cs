using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Facade.Interface;
using APIGastroLink.Hubs;
using APIGastroLink.Models;
using APIGastroLink.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/pedido")]
    [Authorize]
    public class PedidoController : ControllerBase {
        private IDAOPedido _daoPedido;

        private IPedidoService _pedidoService;
        private readonly IHubContext<PedidoHub> _hubContext;

        private readonly IFacadePedido _facadePedido;

        public PedidoController(IDAOPedido daoPedido, IPedidoService pedidoService, IHubContext<PedidoHub> hubContext, IFacadePedido facadePedido) {
            _daoPedido = daoPedido;
            _pedidoService = pedidoService;
            _hubContext = hubContext;
            _facadePedido = facadePedido;
        }

        //GET api-gastrolink/pedido/todos-aberto
        [HttpGet("todos-aberto")]
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
                        Status = i.Status,
                        Observacoes = string.IsNullOrEmpty(i.Observacoes) ? "" : i.Observacoes
                    }).ToList()
                }).ToList();

                return Ok(pedidosResponse);
            } catch (Exception ex) {
                return BadRequest($"Erro ao recuperar pedidos: {ex.Message}");
            }
        }

        //GET api-gastrolink/pedido/todos-finalizado
        [HttpGet("todos-finalizado")]
        public async Task<ActionResult<IEnumerable<PedidoResponseDTO>>> GetPedidosFinalizados() {
            try {
                var pedidos = (await _daoPedido.SelectAll()).Cast<Pedido>().Where(p => p.Status == "FINALIZADO").ToList();

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
                        Status = i.Status,
                        Observacoes = string.IsNullOrEmpty(i.Observacoes) ? "" : i.Observacoes
                    }).ToList()
                }).ToList();

                return Ok(pedidosResponse);
            } catch (Exception ex) {
                return BadRequest($"Erro ao recuperar pedidos: {ex.Message}");
            }
        }


        //POST api-gastrolink/pedido
        [HttpPost]
        public async Task<ActionResult<PedidoResponseDTO>> PostPedido([FromBody] PedidoRequestDTO PedidoCreateDTO) {
            if (PedidoCreateDTO == null) {
                return BadRequest("Dados invalidos.");
            }

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (usuarioId == 0) {
                return Unauthorized("Usuario nao autenticado.");
            }

            try {
                var pedidoResponse = _facadePedido.SalvarPedido(PedidoCreateDTO, usuarioId).Result;

                await _hubContext.Clients.All.SendAsync("NovoPedido", pedidoResponse);

                return Ok(pedidoResponse);
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

        //DELETE api-gastrolink/pedido/{pedidoId}
        [HttpDelete("{pedidoId}")]
        public async Task<ActionResult> DeletePedido(int pedidoId) {
            try {
                await _facadePedido.ExcluirPedido(pedidoId);
                return NoContent();
            } catch (Exception ex) {
                return BadRequest($"Falha ao excluir o pedido: {ex.Message}");
            }
        }

       //PUT api-gastrolink/pedido/{pedidoId}/finalizar
        [HttpPut("{pedidoId}/finalizar")]
        public async Task<ActionResult> FinalizarPedido(int pedidoId) {
            try {
                await _facadePedido.FinalizarPedido(pedidoId);

                var pedidoResponse = _facadePedido.PegarPedidoPorId(pedidoId).Result;

                await _hubContext.Clients.All.SendAsync("NovoPedidoFinalizado", pedidoResponse);

                return NoContent();
            } catch (Exception ex) {
                return BadRequest($"Falha ao finalizar o pedido: {ex.Message}");
            }
        }
    }
}
