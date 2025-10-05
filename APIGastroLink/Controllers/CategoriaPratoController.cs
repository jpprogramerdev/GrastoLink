using APIGastroLink.DAO;
using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
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
        public async Task<ActionResult<IEnumerable<CategoriaPratoDTO>>> GetCategoriasPrato() {
            var categoriasPrato = (await _daoCategoriaPrato.SelectAll()).Cast<CategoriaPrato>()
                .Select(
                    c => new CategoriaPratoDTO {
                        Id = c.Id,
                        Categoria = c.Categoria,
                        Pratos = c.Pratos.Select(p => new PratoDTO {
                            Id = p.Id,
                            Nome = p.Nome,
                            Descricao = p.Descricao,
                            Preco = p.Preco,
                            TempoMedioPreparo = p.TempoMedioPreparo,
                            Disponivel = p.Disponivel
                        }).ToList()
                    }
                ).ToList();

            return Ok(categoriasPrato);
        }

        //GET api-gastrolink/categoria-prato/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaPratoDTO>> GetById(int id) {
            if (id == 0) {
                return BadRequest("É obrigatório informar o id");
            }

            var categoriaPrato = (CategoriaPrato)(await _daoCategoriaPrato.SelectById(id));

            if (categoriaPrato == null) {
                return NotFound("Categoria não encontrada");
            }

            var categoriaPratoDTO = new CategoriaPratoDTO {
                Id = categoriaPrato.Id,
                Categoria = categoriaPrato.Categoria,
                Pratos = categoriaPrato.Pratos.Select(p => new PratoDTO {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    Preco = p.Preco,
                    TempoMedioPreparo = p.TempoMedioPreparo,
                    Disponivel = p.Disponivel
                }).ToList()
            };

            return Ok(categoriaPratoDTO);
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

        //PUT api-gastrolink/categoria-prato
        [HttpPut]
        public async Task<ActionResult> PutCategoriaPrato([FromBody] CategoriaPrato CategoriaPrato) {
            if(CategoriaPrato == null) {
                return BadRequest("Dado inválidos");
            }
            
            try {
                await _daoCategoriaPrato.Update(CategoriaPrato);
                return Ok("Sucesso ao atualizar a categoria de pratos");
            } catch (KeyNotFoundException KeyEx) {
                return BadRequest($"Falha ao tentar atualizar a categoria de prato: ID da URL não confere com o do objeto enviado. Detalhes: {KeyEx.Message}");
            } catch (Exception ex) {
                return BadRequest($"Falha ao tentar atualizar a categoria de prato: {ex.Message}");
            }
        }

        //DELETE api-gastrolink/categoria-prato/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoriaPrato(int id) {
            try {
                await _daoCategoriaPrato.Delete(new CategoriaPrato { Id = id });
                return NoContent();
            } catch (Exception ex) {
                return BadRequest($"Falha ao tentar excluir a categoria de pratos. Detalhes: {ex.Message}");
            }
        }
    }
}
