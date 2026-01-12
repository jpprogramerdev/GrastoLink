using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCGastroLink.DTO;
using MVCGastroLink.ViewsModel;
using System.Net.Http.Headers;

namespace MVCGastroLink.Controllers {
    [Authorize]
    public class PagamentoController : Controller {
        public IHttpClientFactory _httpClientFactory;

        public PagamentoController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        [Authorize(Roles = "CAIXA")]
        public async Task<IActionResult> GerarPagamento(PagamentoRequestDTO PagamentoRequestDTO) {
            if (!ModelState.IsValid) {
                return RedirectToAction("DetalhesPedido", "Pedido", new { pedidoId = PagamentoRequestDTO.PedidoId });
            }

            var client = _httpClientFactory.CreateClient("ApiGastroLink");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",                HttpContext.Session.GetString("JWToken"));

            var response = await client.PostAsJsonAsync("pagamento", PagamentoRequestDTO);

            if (!response.IsSuccessStatusCode) {
                TempData["FalhaGerarPagamento"] = "Erro ao processar pagamento";
                return RedirectToAction("DetalhesPedido", "Pedido", new { pedidoId = PagamentoRequestDTO.PedidoId });
            }

            TempData["SucessoGerarPagamento"] = $"Pagamento do pedido {PagamentoRequestDTO.PedidoId} processado com sucesso";

            return RedirectToAction("Caixa", "Caixa");
        }
    }
}
