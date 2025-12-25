using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace APIGastroLink.Hubs {
    [Authorize]
    public class PedidoHub : Hub {

    }
}
