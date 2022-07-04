using Exercicios.Estatico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exercicios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtExemploController : ControllerBase
    {
        [HttpGet("{usuario}/{senha}")]
        public ActionResult<string> Get(string usuario, string senha)
        {
            if (!usuario.ToLower().Equals("admin"))
            {
                return Ok("Erro no campo usuário");
            }

            if (!senha.ToLower().Equals("123456"))
            {
                return Ok("Erro no campo senha");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("id", usuario),
                    new Claim(ClaimTypes.Role, "Pessoa")
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = JwtConfiguracao.Issuer,
                Audience = JwtConfiguracao.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(JwtConfiguracao.Key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        [HttpGet("{usuario}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> Get(string usuario)
        {
            Request.Headers.TryGetValue("Authorization", out var headerAuthorization);
            var handler = new JwtSecurityTokenHandler();
            var readJwtToken = handler.ReadJwtToken(headerAuthorization.FirstOrDefault()!.Replace("Bearer ", ""));

            return Ok(new
            {
                UsuarioInformado = $"Usuário {usuario} autenticado com sucesso",
                JWTRecebidoHeader = $"{headerAuthorization}",
                ReadJwtToken = readJwtToken
            });
        }
    }
}
