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
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios() {
            var usuarios = (await _daoUsuario.SelectAll()).Cast<Usuario>();

            var usuariosDTO = usuarios.Select(u => new UsuarioDTO {
                Id = u.Id,
                Nome = u.Nome,
                CPF = u.CPF,
                Ativo = u.Ativo,
                TipoUsuario = u.TipoUsuario?.Tipo,
            });

            return Ok(usuariosDTO);
        }

        // GET api-gastrolink/usuarios/id
        [HttpGet("{id}")]

        public async Task<ActionResult<UsuarioDTO>> GetUsuarioById(int id) {
            if (id == 0) {
                return BadRequest("Id é obrigatório");
            }

            var usuario = (Usuario)(await _daoUsuario.SelectById(id));

            if(usuario == null) {
                return NotFound("Usuario não encontrado");

            }

            var usuarioDTO = new UsuarioDTO{
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                Ativo = usuario.Ativo,
                TipoUsuario = usuario.TipoUsuario?.Tipo,
            };

            return Ok(usuarioDTO);
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

        //DELETE api-gastrolink/usuarios/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id) {
            if (id == 0) { 
                return BadRequest("Id é obrigatório");
            }

            var usuario = (Usuario)(await _daoUsuario.SelectById(id));

            if(usuario == null) {
                return NotFound("Usuario não encontrado");
            }

            try { 
                await _daoUsuario.Delete(new Usuario { Id = id });
                return Ok("Usuario inativado com sucesso");
            } catch (Exception ex) {
                return BadRequest($"Falha ao tentar inativar usuario {ex.Message}");
            }
        }

        //PUT api-gastrolink/usuarios
        [HttpPut]
        public async Task<ActionResult> UpdateUsuario([FromBody] UsuarioUpdateDTO UsuarioDTO) {
            if (UsuarioDTO == null) {
                return BadRequest("Dados ínválidos");
            }

            var Usuario = new Usuario {
                Id = UsuarioDTO.Id,
                Nome = UsuarioDTO.Nome.ToUpper().Trim(),
                CPF = UsuarioDTO.CPF,
                Senha = UsuarioDTO.Senha,
                Ativo = UsuarioDTO.Ativo,
                TipoUsuarioId = UsuarioDTO.TipoUsuarioId
            };

            try {
                await _daoUsuario.Update(Usuario);
                return Ok("Sucesso ao atualizar o usuario");
            }catch(KeyNotFoundException KeyEx) {
                return BadRequest($"Falha ao tentar atualizar o usuario: ID da URL não confere com o do objeto enviado.");
            } catch (Exception ex) {
                return BadRequest($"Falha ao tentar atualizar o usuario: {ex.Message}");
            }
        }
    }
}
