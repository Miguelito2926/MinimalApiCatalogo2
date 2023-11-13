
using Microsoft.AspNetCore.Authorization;
using MinimalApiCatalogo2.Models;
using MinimalApiCatalogo2.Services;

namespace MinimalApiCatalogo2.ApiEndpoints;

public static class AutenticacaoEdnpoints
{
    public static void MapAutenticacaoEndpoints(this WebApplication app)
    {

        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel == null)
            {
                return Results.BadRequest("Login Inválido");
            }
            if (userModel.UserName == "ednaldo" && userModel.Password == "teste@123")
            {
                var tokenString = tokenService.GerarToken(
                    app.Configuration["jwt:Key"],
                    app.Configuration["Jwt:Issuer"],
                    app.Configuration["Jwt:Audience"], userModel);
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Login Inválido");
            }
        }).Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status200OK)
                    .WithName("Login")
                    .WithTags("Autenticacao");

    }

}
