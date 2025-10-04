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
    }
}
