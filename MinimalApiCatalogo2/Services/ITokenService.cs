// Importando o namespace do modelo de dados
using MinimalApiCatalogo2.Models;

// Definição da interface ITokenService
namespace MinimalApiCatalogo2.Services
{
    public interface ITokenService
    {
        // Método para gerar um token JWT com base nos parâmetros fornecidos
        string GerarToken(string key, string issuer, string audience, UserModel user);
    }
}
