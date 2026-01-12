using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    public class FormaPagamentoController : ControllerBase {
        private readonly IDAOFormaPagamento _daoFormaPagamento;

        public FormaPagamentoController(IDAOFormaPagamento daoFormaaPagamento) {
            _daoFormaPagamento = daoFormaaPagamento;
        }

        [HttpGet]
        [Route("api-gastrolink/formas-pagamento")]
        public async Task<ActionResult<IEnumerable<FormaPagamentoDTO>>> GetAllFormaPagamento() {
            try {
                var listFormaPagamento = (await _daoFormaPagamento.SelectAll()).Cast<FormaPagamento>().ToList() ;
                
                var formasPagamentoDTO = new List<FormaPagamentoDTO>();

                foreach(var form in listFormaPagamento) {
                    formasPagamentoDTO.Add(new FormaPagamentoDTO {
                        Id = form.Id,
                        Forma = form.Forma
                    });
                }
                return Ok(formasPagamentoDTO);
            } catch (Exception ex) {
                return BadRequest($"Falha ao buscar formas de pagamento: {ex.Message}");
            }
        }
    }
}
