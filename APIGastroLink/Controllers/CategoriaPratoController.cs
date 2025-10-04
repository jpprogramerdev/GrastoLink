using APIGastroLink.DAO.Interface;
using APIGastroLink.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/categoria-prato")]
    public class CategoriaPratoController : ControllerBase {
        private IDAOCategoriaPrato _daoCategoriaPrato;

        public CategoriaPratoController(IDAOCategoriaPrato daoCategoriraPrato) {
            _daoCategoriaPrato = daoCategoriraPrato;
        }

        //GET api-gastrolink/categoria-prato
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaPrato>>> GetCategoriasPrato() {
            var categoriasPrato = (await _daoCategoriaPrato.SelectAll()).Cast<CategoriaPrato>().ToList();

            return Ok(categoriasPrato);
        }

        //GET api-gastrolink/categoria-prato/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaPrato>> GetById(int id) {
            if(id == 0) {
                return BadRequest("É obrigatório informar o id");
            }

            var categoriaPrato = (CategoriaPrato)(await _daoCategoriaPrato.SelectById(id));

            if(categoriaPrato == null) {
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoriaPrato);
        }

        //POST api-gastrolink/categoria-prato
        [HttpPost]
        public async Task<ActionResult> PostCategoriaPrato([FromBody] CategoriaPrato CategoriaPrato) {
            if (CategoriaPrato == null) {
                return BadRequest("Dados invalidos");
            }

            try {
                await _daoCategoriaPrato.Insert(CategoriaPrato);
            } catch (Exception ex) {
                return BadRequest("Falha ao inserir a catagoria de dados");
            }

            return Created("Categoria prato criado", CategoriaPrato);
        }

        
    }
}
