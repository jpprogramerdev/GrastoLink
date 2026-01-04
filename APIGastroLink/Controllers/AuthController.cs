using APIGastroLink.DAO.Interface;
using APIGastroLink.DTO;
using APIGastroLink.Models;
using APIGastroLink.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace APIGastroLink.Controllers {
    [ApiController]
    [Route("api-gastrolink/auth")]
    public class AuthController : Controller {
        private readonly ITokenService _tokenService;
        private readonly IDAOUsuario _daoUsuario;

        public AuthController(ITokenService tokenService, IDAOUsuario daoUsuario) {
            _tokenService = tokenService;
            _daoUsuario = daoUsuario;
        }



        //POST api-gastrolink/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> AutenticacaoUsuario(Login Login) {
            if (Login == null || string.IsNullOrEmpty(Login.CPF) || string.IsNullOrEmpty(Login.Senha)) {
                return BadRequest(new MessagemResponseDTO { Mensagem = "Dados de login inválidos" });
            }

            var usuario = (await _daoUsuario.Authenticate(Login.CPF, Login.Senha)) as Usuario;
            if (usuario == null) {
                return Unauthorized(new MessagemResponseDTO { Mensagem = "CPF e/ou senha incorretos" });
            }

            var usuarioDTO = new UsuarioDTO {
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                Ativo = usuario.Ativo,
                TipoUsuario = usuario.TipoUsuario?.Tipo,
            };

            var token = _tokenService.GenerateToken(usuarioDTO.Id.ToString(), usuarioDTO.Nome);

            var expiracaoToken = int.Parse(HttpContext.RequestServices.GetService<IConfiguration>()["Jwt:ExpireMinutes"] ?? "60");

            return Ok(new LoginResponse {
                Token = token,
                Usuario = usuarioDTO
            });
        }

    }
}
