using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using InssApi.Models;
using InssApi.Data;
using InssApi.Middleware;
using InssApi.Services;
using InssApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;



namespace InssApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _cfg;

    public AuthController(IConfiguration cfg) => _cfg = cfg;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        if (req.Email != "ana@inss.gov.mz" || req.Senha != "1234")
            return Unauthorized("E-mail ou senha inválidos.");
        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_cfg["Jwt:Chave"]!));

        var cred = new SigningCredentials(
            chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Emissor"],
            claims: new[]
            {
                new Claim(ClaimTypes.Name, "Ana Mucavel"),
                new Claim(ClaimTypes.Email, req.Email)
            },
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: cred);
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            nome = "Ana Mucavel",
            email = req.Email
        });
    }

}
