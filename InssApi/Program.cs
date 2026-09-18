using InssApi.Data;
using InssApi.Middleware;
using InssApi.Repositories;
using InssApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

// Dia 3 · manhã · M13
// Parte do InssApi da tarde do Dia 2 (M8: camadas + middleware).
// Hoje NÃO mudamos Controller/Service/Repository — só abrimos a porta para o React.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Igual ao M8 — DI das camadas
builder.Services.AddScoped<IContribuinteRepository, ContribuinteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IContribuinteService, ContribuinteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// M13 — CORS: o browser trata 5173 e 5088 como sites diferentes.
// Sem isso o portal recebe "blocked by CORS policy" no F12.
builder.Services.AddCors(o => o.AddPolicy("portal", p =>
    p.WithOrigins("*")
     .AllowAnyHeader()
     .AllowAnyMethod()));

var chaveJwt = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Chave"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Emissor"],
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = chaveJwt
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ErrosMiddleware>(); // M8 — continua

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Em sala: NÃO UseHttpsRedirection — o portal chama http://localhost:5088.
// Com redirecionamento, o CORS quebra no meio do caminho.
app.UseCors("portal");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Preciso para WebApplicationFactory nos testes de integração (M15)
public partial class Program { }