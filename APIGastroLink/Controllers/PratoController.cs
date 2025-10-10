using APIGastroLink.DAO;
using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/prato")]
    public class PratoController : ControllerBase {
        private readonly IDAOPrato _daoPrato;

        public PratoController(IDAOPrato daoPrato) {
            _daoPrato = daoPrato;
        }

        //GET api-gastrolink/prato
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PratoDTO>>> GetPratos() {
            var pratos = (await _daoPrato.SelectAll()).Cast<Prato>().ToList();
            var pratosDtos = pratos.Select(p => new PratoDTO {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                TempoMedioPreparo = p.TempoMedioPreparo,
                Disponivel = p.Disponivel,
                CategoriaPrato = new CategoriaPratoDTO {
                    Id = p.CategoriaPrato.Id,
                    Categoria = p.CategoriaPrato.Categoria
                }
            });

            return Ok(pratosDtos);
        }

        //GET api-gastrolink/prato/id
        [HttpGet("{id}")]
        public async Task<ActionResult<PratoDTO>> GetPratoById(int id) {
            if (id == 0) {
                return BadRequest("É obrigatório informar o id");
            }

            var prato = (Prato)(await _daoPrato.SelectById(id));


            if (prato == null) {
                return NotFound("Prato não encontrado");
            }

            var pratoDto = new PratoDTO {
                Id = prato.Id,
                Nome = prato.Nome,
                Descricao = prato.Descricao,
                Preco = prato.Preco,
                TempoMedioPreparo = prato.TempoMedioPreparo,
                Disponivel = prato.Disponivel,
                CategoriaPrato = new CategoriaPratoDTO {
                    Id = prato.CategoriaPrato.Id,
                    Categoria = prato.CategoriaPrato.Categoria
                }
            };

            return Ok(pratoDto);
        }

        //POST api-gastrolink/prato
        [HttpPost]
        public async Task<ActionResult<Prato>> PostPrato([FromBody] PratoCreateDTO PratoCreateDTO) {
            if (PratoCreateDTO == null) { 
                return BadRequest("Dados invalidos") ;
            }

            var prato = new Prato {
                Nome  = PratoCreateDTO.Nome,
                Descricao = PratoCreateDTO.Descricao,
                Preco = PratoCreateDTO.Preco,
                TempoMedioPreparo = PratoCreateDTO.TempoMedioPreparo,
                Disponivel = PratoCreateDTO.Disponivel,
                CategoriaPratoId = PratoCreateDTO.CategoriaPratoId
            };
            
            try {
                await _daoPrato.Insert(prato);
                return Created("Prato criado", prato);
            } catch (Exception ex) {
                return BadRequest($"Falha ao salvar o prato: {ex.Message}");
            }
        }
    }
}
