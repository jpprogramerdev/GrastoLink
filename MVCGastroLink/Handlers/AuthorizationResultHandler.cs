using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace MVCGastroLink.Handlers {
    public class AuthorizationResultHandler : IAuthorizationMiddlewareResultHandler {
        private readonly IAuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult) {
            if (authorizeResult.Forbidden) {
                context.Session.SetString("AcessoNegado","Acesso negado. Você não tem permissão para acessar esta funcionalidade.");

                var ultimaUrl = context.Session.GetString("UltimaUrlValida");

                context.Response.Redirect(!string.IsNullOrEmpty(ultimaUrl) ? ultimaUrl : "/Login/Login");

                return;
            }
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
