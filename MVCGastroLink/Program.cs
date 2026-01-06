using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MVCGastroLink.Handlers;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtHandler>();
builder.Services.AddHttpClient("ApiGastroLink", client => {
    client.BaseAddress = new Uri("https://localhost:7071/api-gastrolink/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<JwtHandler>();

builder.Services.AddControllersWithViews(options => {
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationResultHandler>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options => {
    options.LoginPath = "/Login/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;

    options.Events = new CookieAuthenticationEvents {
        OnRedirectToAccessDenied = context => {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();


app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) => {
    await next();

    if (context.Request.Method == "GET" && context.Response.StatusCode == StatusCodes.Status200OK && context.User.Identity?.IsAuthenticated == true) {
        var path = context.Request.Path + context.Request.QueryString;

        if (!path.StartsWith("/Login") && !path.StartsWith("/Erro") && !path.StartsWith("/Account") &&
            !path.StartsWith("/css") && !path.StartsWith("/js") && !path.StartsWith("/lib")) {
            context.Session.SetString("UltimaUrlValida", path);
        }
    }
});


app.UseStatusCodePages(async context => {
    var response = context.HttpContext.Response;

    if (response.StatusCode == StatusCodes.Status403Forbidden) {
        var session = context.HttpContext.Session;

        session.SetString(
            "AcessoNegado",
            "Acesso negado. Você não tem permissão para acessar esta funcionalidade."
        );

        var ultimaUrl = session.GetString("UltimaUrlValida");

        response.Redirect(!string.IsNullOrEmpty(ultimaUrl) ? ultimaUrl : "/Login/Login");
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
