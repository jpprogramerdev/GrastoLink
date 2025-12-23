namespace MVCGastroLink.Handlers {
    public class JwtHandler : DelegatingHandler{
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtHandler(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt_token"];

            if (!string.IsNullOrEmpty(token)) {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
