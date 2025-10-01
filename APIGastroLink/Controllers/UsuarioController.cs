using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/usuario")]
    public class UsuarioController : ControllerBase {
        private readonly IDAOUsuario _daoUsuario;

        public UsuarioController(IDAOUsuario daoUsuario) {
            _daoUsuario = daoUsuario;
        }
        
        //GET api-gastrolink/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios() {
            var usuarios = (await _daoUsuario.SelectAll()).Cast<Usuario>();

            var usuariosDTO = usuarios.Select(u => new UsuarioDTO {
                Id = u.Id,
                Nome = u.Nome,
                CPF = u.CPF,
                Ativo = u.Ativo,
                Tipo = u.TipoUsuario?.Tipo,
            });

            return Ok(usuariosDTO);
        }

        //POST api-gastrolink/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioCreateDTO UsuarioDTO) {
            if (UsuarioDTO == null) {
                return BadRequest("Dados ínválidos");
            }

            var Usuario = new Usuario {
                Nome = UsuarioDTO.Nome.ToUpper().Trim(),
                CPF = UsuarioDTO.CPF,
                Senha = UsuarioDTO.Senha,
                Ativo = UsuarioDTO.Ativo,
                TipoUsuarioId = UsuarioDTO.TipoUsuarioId
            };

            await _daoUsuario.Insert(Usuario);

            return Created("Usuario criado com sucesso", Usuario);
        }
    }
}
