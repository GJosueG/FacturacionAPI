using FacturacionAPI.Endpoints;
using FacturacionAPI.Models;
using FacturacionAPI.Services.Categorias;
using FacturacionAPI.Services.Estados;
using FacturacionAPI.Services.Facturas;
using FacturacionAPI.Services.Productos;
using FacturacionAPI.Services.Roles;
using FacturacionAPI.Services.Tickets;
using FacturacionAPI.Services.Usuarios;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FacturasDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("FacturaDbConnection"))
);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<ICategoriaServices, CategoriaServices>();
builder.Services.AddScoped<IEstadoServices, EstadoServices>();
builder.Services.AddScoped<IRolServices, RolServices>();
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();
builder.Services.AddScoped<IProductoServices, ProductoServices>();
builder.Services.AddScoped<ITicketServices, TicketServices>();
builder.Services.AddScoped<IFacturaServices, FacturaServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseEnpoints();
app.Run();
