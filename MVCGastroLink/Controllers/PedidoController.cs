using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCGastroLink.DTO;
using System.Threading.Tasks;

namespace MVCGastroLink.Controllers {
    [Authorize]
    public class PedidoController : Controller {
        private readonly IHttpClientFactory _httpClientFactory;

        public PedidoController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> TodosPedidos() {

            var client = _httpClientFactory.CreateClient("ApiGastroLink");

            var response = await client.GetAsync("pedido/todos-aberto");

            if (!response.IsSuccessStatusCode) {
                ViewData["FalhaBuscaTodoPedidos"] = "Erro ao buscar pedidos";
                return View(new List<PedidoResponseDTO>());
            }

            var pedidoContent = await response.Content.ReadFromJsonAsync<List<PedidoResponseDTO>>();


            return View(pedidoContent);
        }
    }
}
