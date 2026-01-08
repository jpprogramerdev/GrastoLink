using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCGastroLink.DTO;
using MVCGastroLink.ViewsModel;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVCGastroLink.Controllers {
    [Authorize]
    public class PedidoController : Controller {
        private readonly IHttpClientFactory _httpClientFactory;

        public PedidoController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "GARÇOM")]
        public async Task<IActionResult> CriarPedido() {

            var criarPedidoViewlModel = new CriarPedidoViewlModel();

            var client = _httpClientFactory.CreateClient("ApiGastroLink");

            var token = HttpContext.Session.GetString("JWToken");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var mesaResponse = await client.GetAsync("mesa/mesas-livres");

            if (!mesaResponse.IsSuccessStatusCode) {
                ViewData["FalhaBuscaMesas"] = "Erro ao buscar mesas";
                return View(criarPedidoViewlModel);
            }

            criarPedidoViewlModel.MesasDTO = await mesaResponse.Content.ReadFromJsonAsync<List<MesaDTO>>();

            var pratoResponse = await client.GetAsync("prato");

            if (!pratoResponse.IsSuccessStatusCode) {
                ViewData["FalhaBuscaPratos"] = "Erro ao buscar pratos";
                return View(criarPedidoViewlModel);
            }

            criarPedidoViewlModel.PratosDTO = await pratoResponse.Content.ReadFromJsonAsync<List<PratoDTO>>();

            criarPedidoViewlModel.PedidoRequestDTO = new PedidoRequestDTO();

            if (criarPedidoViewlModel.MesasDTO == null || criarPedidoViewlModel.PratosDTO == null) {
                TempData["FalhaCriarPedido"] = "Erro ao carregar dados para criar pedido";
                return RedirectToAction("TodosPedidos", "Pedido");
            }

            return View(criarPedidoViewlModel);
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido(CriarPedidoViewlModel criarPedidoViewlModel) {
            try {
                if (!ModelState.IsValid) {
                    TempData["FalhaCriarPedido"] = "Dados invalidos do pedido";
                    return RedirectToAction("CriarPedido");
                }

                var token = HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(token)) {
                    TempData["FalhaCriarPedido"] = "Usuário não autenticado";
                    return RedirectToAction("Login", "Login");
                }

                var client = _httpClientFactory.CreateClient("ApiGastroLink");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync("pedido", criarPedidoViewlModel.PedidoRequestDTO);

                if (!response.IsSuccessStatusCode) {
                    TempData["FalhaCriarPedido"] = "Erro ao criar pedido";
                    return RedirectToAction("CriarPedido");
                }

                TempData["SucessoCriarPedido"] = "Pedido criado com sucesso!";
                return RedirectToAction("CriarPedido");
            } catch (NullReferenceException nllEx) {
                TempData["FalhaCriarPedido"] = "Erro ao carregar dados para criar pedido";
                return RedirectToAction("TodosPedidos", "Pedido");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcluirPedido(int pedidoId) {
            if (pedidoId == 0) {
                TempData["FalhaExclusaoPedido"] = "ID do pedido inválido";
                return RedirectToAction("TodosPedidos");
            }

            var client = _httpClientFactory.CreateClient("ApiGastroLink");

            var token = HttpContext.Session.GetString("JWToken");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"pedido/{pedidoId}");

            if (!response.IsSuccessStatusCode) {
                TempData["FalhaExclusaoPedido"] = "Erro ao excluir pedido";
                return RedirectToAction("TodosPedidos");
            }

            TempData["SucessoExclusaoPedido"] = "Pedido excluído com sucesso!";

            return RedirectToAction("TodosPedidos");
        }


        [HttpGet]
        public async Task<IActionResult> FinalizarPedido(int pedidoId) {
            if (pedidoId == 0) {
                TempData["FalhaDetalhesPedido"] = "ID do pedido inválido";
                return RedirectToAction("TodosPedidos");
            }
            var client = _httpClientFactory.CreateClient("ApiGastroLink");
            var token = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PutAsync($"pedido/{pedidoId}/finalizar",null);

            if (!response.IsSuccessStatusCode) {
                TempData["FalhaDetalhesPedido"] = "Erro ao buscar detalhes do pedido";
                return RedirectToAction("TodosPedidos");
            }
            return RedirectToAction("TodosPedidos");
        }

        [Authorize(Roles = "COZINHEIRO")]
        public async Task<IActionResult> TodosPedidos() {

            var client = _httpClientFactory.CreateClient("ApiGastroLink");

            var token = HttpContext.Session.GetString("JWToken");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("pedido/todos-aberto");

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


        [Authorize(Roles = "GARÇOM")]
        public async Task<IActionResult> PedidosFinalizados() {
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
