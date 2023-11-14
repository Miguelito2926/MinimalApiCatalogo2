// Importando namespaces necessários
using Microsoft.IdentityModel.Tokens;
using MinimalApiCatalogo2.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// Definição do namespace e da classe TokenService que implementa ITokenService
namespace MinimalApiCatalogo2.Services
{
    public class TokenService : ITokenService
    {
        // Método para gerar um token JWT com base nos parâmetros fornecidos
        public string GerarToken(string key, string issuer, string audience, UserModel user)
        {
            // Criando as reivindicações (claims) do token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            // Criando a chave de segurança com base na chave fornecida
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Criando as credenciais para assinar o token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Criando o token JWT com as informações fornecidas
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials
            );

            // Criando um manipulador de token JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // Escrevendo o token como uma string
            var stringToken = tokenHandler.WriteToken(token);

            // Retornando a string do token gerado
            return stringToken;
        }
    }
}
