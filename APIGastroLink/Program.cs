using APIGastroLink.Context;
using APIGastroLink.DAO;
using APIGastroLink.DAO.Interface;
using APIGastroLink.Services;
using APIGastroLink.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDAOUsuario, DAOUsuario>();
builder.Services.AddTransient<IDAOMesa, DAOMesa>();
builder.Services.AddTransient<IDAOCategoriaPrato, DAOCategoriaPrato>();
builder.Services.AddTransient<IDAOPrato, DAOPrato>();
builder.Services.AddTransient<IDAOPedido, DAOPedido>();

builder.Services.AddTransient<IPedidoService, PedidoService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
