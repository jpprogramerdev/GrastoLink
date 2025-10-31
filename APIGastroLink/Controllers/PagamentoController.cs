using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/pagamento")]
    public class PagamentoController : ControllerBase {
        private readonly IDAOPagamento _daoPagamento;

        public PagamentoController(IDAOPagamento daoPagamento) {
            _daoPagamento = daoPagamento;
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
    }
}
