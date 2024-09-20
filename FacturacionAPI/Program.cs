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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    { 
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Ingresa el token JWT en el siguiente formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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

var jwtSettings = builder.Configuration.GetSection("JwtSetting");
var secretkey = jwtSettings.GetValue<string>("secretkey");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(
    options => {
        //Esquema por defecto
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
   options => {
       //Permite usar HTTP en lugar HTTPS
       options.RequireHttpsMetadata = false;
       //Guardar el token en el contexto de autenticación
       options.SaveToken = true;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
           ValidAudience = jwtSettings.GetValue<string>("Audience"),
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey))
       };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints();
app.Run();
