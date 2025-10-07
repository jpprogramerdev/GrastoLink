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
                    Id  = p.CategoriaPrato.Id,
                    Categoria = p.CategoriaPrato.Categoria
                }
            });

            return Ok(pratosDtos);
        }

        //GET api-gastrolink/prato/id

    }
}
