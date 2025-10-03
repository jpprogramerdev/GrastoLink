using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    [Route("api-gastrolink/mesa")]
    [ApiController]
    public class MesaController : ControllerBase {
        private readonly IDAOMesa _daoMesa;

        public MesaController(IDAOMesa daoMesa) {
            _daoMesa = daoMesa;
        }

        //GET api-gastrolink/mesa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MesaDTO>>> GetMesas() {
            var mesas = (await _daoMesa.SelectAll()).Cast<Mesa>().ToList();
            var mesasDTO = mesas.Select(m => new MesaDTO {
                Id = m.Id,
                Numero = m.Numero,
                Status = m.Status.ToString(),
                Pedidos = m.Pedidos.ToList()
            });

            return Ok(mesasDTO);
        }

        //POST
        [HttpPost]
        public async Task<ActionResult<Mesa>> PostMesa([FromBody] Mesa Mesa) {
            if (Mesa == null) {
                return BadRequest("Dados invalidos");
            }

            try {
                await _daoMesa.Insert(Mesa);
            } catch (Exception ex) {
                return BadRequest($"Falha ao cadastra mesa nova: {ex.Message}");
            }

            return Created("Sucesso ao cadastrar a mesa", Mesa);
        }
    }
}
