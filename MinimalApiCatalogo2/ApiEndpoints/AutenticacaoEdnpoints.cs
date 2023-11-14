// Importando namespaces necessários
using Microsoft.AspNetCore.Authorization;
using MinimalApiCatalogo2.Models;
using MinimalApiCatalogo2.Services;

namespace MinimalApiCatalogo2.ApiEndpoints
{
    public static class AutenticacaoEdnpoints
    {
        // Método para mapear os endpoints de autenticação
        public static void MapAutenticacaoEndpoints(this WebApplication app)
        {
            // Mapeando um endpoint de login usando o método HTTP POST
            app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
            {
                // Verificando se o modelo do usuário é nulo
                if (userModel == null)
                {
                    return Results.BadRequest("Login Inválido");
                }

                // Verificando credenciais do usuário (exemplo simplificado, deve ser melhorado em produção)
                if (userModel.UserName == "ednaldo" && userModel.Password == "teste@123")
                {
                    // Gerando token se as credenciais estiverem corretas
                    var tokenString = tokenService.GerarToken(
                        app.Configuration["jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"], userModel);

                    // Retornando o token no corpo da resposta se o login for bem-sucedido
                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    // Retornando uma resposta de erro se as credenciais forem inválidas
                    return Results.BadRequest("Login Inválido");
                }
            })
            // Especificando os códigos de status que o endpoint pode produzir
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            // Nomeando o endpoint como "Login" para referência
            .WithName("Login")
            // Adicionando tags para categorizar o endpoint (nesse caso, "Autenticacao")
            .WithTags("Autenticacao");
        }
    }
}
