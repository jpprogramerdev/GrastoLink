using APIGastroLink.DTO;
using APIGastroLink.Mapper.Interface;
using APIGastroLink.Models;

namespace APIGastroLink.Mapper {
    public class PedidoMapper : IPedidoMapper {
        public PedidoResponseDTO ToDTO(Pedido pedido) => new PedidoResponseDTO {
            Id = pedido.Id,
            DataHora = pedido.DataHora,
            Status = pedido.Status,
            Mesa = new MesaDTO {
                Id = pedido.Mesa.Id,
                Numero = pedido.Mesa.Numero,
                Status = pedido.Mesa.Status.ToString()
            },
            UsuarioId = pedido.UsuarioId,
            ValorTotal = pedido.ValorTotal,
            Itens = pedido.ItensPedido.Select(i => new ItemPedidoResponseDTO {
                Prato = new PratoDTO {
                    Nome = i.Prato.Nome,
                },
                Quantidade = i.Quantidade,
                Status = i.Status
            }).ToList()
        };
        public Pedido ToEntity(PedidoRequestDTO dto, int usuarioId) => new Pedido {
            DataHora = DateTime.Now,
            Status = "ABERTO",
            MesaId = dto.MesaId,
            UsuarioId = usuarioId,
            ItensPedido = dto.ItensPedido.Select(i => new ItemPedido {
                PratoId = i.PratoId,
                Quantidade = i.Quantidade,
                Status = "PENDENTE"
            }).ToList()
        };

    }
}
