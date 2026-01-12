using APIGastroLink.Context;
using APIGastroLink.DAO;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Facade;
using APIGastroLink.Facade.Interface;
using APIGastroLink.Hubs;
using APIGastroLink.Mapper;
using APIGastroLink.Mapper.Interface;
using APIGastroLink.Services;
using APIGastroLink.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Configure JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = jwtConfig["Key"];
var issuer = jwtConfig["Issuer"];
var audience = jwtConfig["Audience"];


//Authentication and Authorization configuration
builder.Services.AddAuthentication(options => { 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime =  true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
    options.Events = new JwtBearerEvents {
        OnMessageReceived = context => {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/pedidoHub"))) {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };  
});

builder.Services.AddCors(options => {
    options.AddPolicy("SignalRCors", policy => {
        policy.WithOrigins("https://localhost:7059")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddScoped<IDAODatabase, DAOSqlServer>();

builder.Services.AddTransient<IDAODatabase, DAOSqlServer>();

builder.Services.AddTransient<IDAOUsuario, DAOUsuario>();
builder.Services.AddTransient<IDAOMesa, DAOMesa>();
builder.Services.AddTransient<IDAOCategoriaPrato, DAOCategoriaPrato>();
builder.Services.AddTransient<IDAOPrato, DAOPrato>();
builder.Services.AddTransient<IDAOPedido, DAOPedido>();
builder.Services.AddTransient<IDAOPagamento, DAOPagamento>();
builder.Services.AddTransient<IDAOFormaPagamento, DAOFormaPagamento>();

builder.Services.AddTransient<IPedidoService, PedidoService>();

builder.Services.AddTransient<IPedidoMapper, PedidoMapper>();

builder.Services.AddScoped<IFacadePedido, FacadePedido>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.CommandTimeout(90)
    ));

builder.Services.AddControllers();

builder.Services.AddScoped<ITokenService, TokenService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

app.UseCors("SignalRCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PedidoHub>("/pedidoHub");

app.Run();
