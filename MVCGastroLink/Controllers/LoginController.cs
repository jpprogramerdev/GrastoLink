using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVCGastroLink.DTO;
using System.Security.Claims;
using System.Text.Json;

namespace MVCGastroLink.Controllers {
    public class LoginController : Controller {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO LoginRequestDTO) {
            if (!ModelState.IsValid) {
                return View(LoginRequestDTO);
            }

            var client = _httpClientFactory.CreateClient("ApiGastroLink");

            var payload = new {
                cpf = LoginRequestDTO.CPF,
                senha = LoginRequestDTO.Senha
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("auth/login", content);

            if (response.IsSuccessStatusCode) {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, loginResponse.Usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, loginResponse.Usuario.Nome),
                    new Claim(ClaimTypes.Role, loginResponse.Usuario.TipoUsuario)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                var responseContent = await response.Content.ReadAsStringAsync();

                var jsonDoc = JsonDocument.Parse(responseContent);

                var token = jsonDoc.RootElement.GetProperty("token").GetString();

                HttpContext.Session.SetString("JWToken", token ?? string.Empty);

                return RedirectToAction("TodosPedidos", "Pedido");
            } else {
                ModelState.AddModelError(string.Empty, "CPF ou senha inválidos.");
                return View(LoginRequestDTO);
            }
        }
    }
}
