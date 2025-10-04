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

        //GET api-gastrolink/mesa/por-numero/{numero}
        [HttpGet("por-numero/{numero}")]
        public async Task<ActionResult<MesaDTO>> GetMesaByNumero(string numero) {
            var mesa = await _daoMesa.SelectByNumero(numero);

            if (mesa == null) {
                return NotFound("Mesa não localizada");
            }

            var mesaDTO = new MesaDTO {
                Id = mesa.Id,
                Numero = mesa.Numero,
                Status = mesa.Status.ToString(),
                Pedidos = mesa.Pedidos.ToList()
            };

            return Ok(mesaDTO);
        }

        //GET api-gastrolink/mesa/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MesaDTO>> GetById (int id) {
            var mesa = (Mesa)(await _daoMesa.SelectById(id));

            if (mesa == null) {
                return NotFound("Mesa não localizada");
            }

            var mesaDTO = new MesaDTO {
                Id = mesa.Id,
                Numero = mesa.Numero,
                Status = mesa.Status.ToString(),
                Pedidos = mesa.Pedidos.ToList()
            };

            return Ok(mesaDTO);
        }

        //POST api-gastrolink/mesa
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
