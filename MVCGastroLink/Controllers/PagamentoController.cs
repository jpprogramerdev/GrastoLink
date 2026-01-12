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

            if(PagamentoRequestDTO.Desconto < 0) {
                TempData["FalhaGerarPagamento"] = "Desconto não pode ser negativo";
                return RedirectToAction("DetalhesPedido", "Pedido", new { pedidoId = PagamentoRequestDTO.PedidoId });
            }

            if (PagamentoRequestDTO.Desconto > PagamentoRequestDTO.ValorTotal) {
                TempData["FalhaGerarPagamento"] = "Desconto não pode ser maior que o valor total da compra";
                return RedirectToAction("DetalhesPedido", "Pedido", new { pedidoId = PagamentoRequestDTO.PedidoId });
            }

            var totalComDesconto = PagamentoRequestDTO.ValorTotal - PagamentoRequestDTO.Desconto;

            if (PagamentoRequestDTO.ValorPago != totalComDesconto) { 
                TempData["FalhaGerarPagamento"] = "Valor pago não pode ser diferente do valor total do pedido sem considerar o desconto";
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
