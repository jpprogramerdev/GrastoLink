using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCGastroLink.DTO;
using System.Net.Http.Headers;

namespace MVCGastroLink.Controllers {
    public class CaixaController : Controller{
        private readonly IHttpClientFactory _httpClientFactory;

        public CaixaController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "CAIXA")]
        public async Task<IActionResult> Caixa() {
            var client = _httpClientFactory.CreateClient("ApiGastroLink");

            var token = HttpContext.Session.GetString("JWToken");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("pedido/todos-finalizado");

            if (!response.IsSuccessStatusCode) {
                TempData["FalhaBuscaTodoPedidos"] = "Erro ao buscar pedidos";
                return View(new List<PedidoResponseDTO>());
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var pedidos = new List<PedidoResponseDTO>();

            if (responseContent.Trim().StartsWith("[")) {
                pedidos = response.Content.ReadFromJsonAsync<List<PedidoResponseDTO>>().Result;
            } else {
                pedidos = new List<PedidoResponseDTO>();
            }

            return View(pedidos);
        }
    }
}
