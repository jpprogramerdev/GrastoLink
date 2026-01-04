using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Facade.Interface;
using APIGastroLink.Hubs;
using APIGastroLink.Mapper.Interface;
using APIGastroLink.Models;
using APIGastroLink.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace APIGastroLink.Facade {
    public class FacadePedido : IFacadePedido {
        private readonly IDAOMesa _daoMesa;
        private readonly IDAOPedido _daoPedido;

        private readonly IPedidoService _pedidoService;
        private readonly IPedidoMapper _pedidoMapper;

        public FacadePedido(IDAOPedido daoPedido, IPedidoService pedidoService, IPedidoMapper pedidoMapper, IDAOMesa daoMesa) {
            _daoPedido = daoPedido;
            _daoMesa = daoMesa;

            _pedidoService = pedidoService;
            _pedidoMapper = pedidoMapper;
        }

        public async Task ExcluirPedido(int pedidoId) {
            var pedido = (await _daoPedido.SelectById(pedidoId)) as Pedido;

            if (pedido == null) {
                throw new Exception("Pedido nao encontrado.");
            }

            try {
                await _daoPedido.Delete(pedido);

                var mesa = (await _daoMesa.SelectById(pedido.MesaId)) as Mesa;

                if (mesa != null) {
                    mesa.Status = StatusMesa.LIVRE;

                    await _daoMesa.Update(mesa);
                } else {
                    throw new Exception("Mesa nao encontrada. Por favor confirme o número da mesa");
                }

            } catch (Exception ex) {
                throw new Exception($"Erro ao excluir pedido: {ex.Message}");
            }
        }

        public async Task FinalizarPedido(int pedidoId) {
            var pedido = await _daoPedido.SelectById(pedidoId) as Pedido;

            if (pedido == null) {
                throw new Exception("Pedido nao encontrado.");
            }

            try {
                await _daoPedido.Update(pedido);
            } catch (Exception ex) {
                throw new Exception($"Erro ao finalizar pedido: {ex.Message}");
            }
        }

        public async Task<PedidoResponseDTO> SalvarPedido(PedidoRequestDTO dto, int usuarioId) {
            var pedidoEntity = _pedidoMapper.ToEntity(dto, usuarioId);

            try {
                await _daoPedido.Insert(pedidoEntity);

                pedidoEntity = (await _daoPedido.SelectById(pedidoEntity.Id)) as Pedido;

                var mesa = (await _daoMesa.SelectById(pedidoEntity.MesaId)) as Mesa;

                if (mesa != null) {
                    mesa.Status = StatusMesa.OCUPADO;

                    await _daoMesa.Update(mesa);
                } else {
                    throw new Exception("Mesa nao encontrada. Por favor confirme o número da mesa");
                }

                 return _pedidoMapper.ToDTO(pedidoEntity);

            } catch (Exception ex) {
                throw new Exception($"Erro ao salvar pedido: {ex.Message}");
            }
        }
    }
}
