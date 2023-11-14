using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo2.Context;
using MinimalApiCatalogo2.Models;

namespace MinimalApiCatalogo2.ApiEndpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
        //---------------ENDPOINTS PARA PRODUTOS---------------------
        app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync()).WithTags("Produtos").RequireAuthorization();

        app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Produtos.FindAsync(id)
                 is Produto produto
                ? Results.Ok(produto)
                : Results.NotFound();
        }).WithTags("Produtos");

        app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
        {
            db.Produtos.Add(produto);
            await db.SaveChangesAsync();
            return Results.Created($"/categorias/{produto.ProdutoId}", produto);
        }).WithTags("Produtos");

        app.MapPut("/produtoss/{id:int}", async (int id, Produto produto, AppDbContext db) =>
        {
            if (produto.ProdutoId != id)
            {
                return Results.BadRequest();
            }
            var produtosDB = await db.Produtos.FindAsync(id);
            if (produtosDB is null) return Results.NotFound();

            produtosDB.Nome = produto.Nome;
            produtosDB.Descricao = produto.Descricao;

            await db.SaveChangesAsync();
            return Results.Ok(produtosDB);
        }).WithTags("Produtos");

        app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
        {
            if (produto.ProdutoId != id)
            {
                return Results.BadRequest();
            }
            var produtoDB = await db.Produtos.FindAsync(id);
            if (produtoDB is null) return Results.NotFound();

            produtoDB.Nome = produto.Nome;
            produtoDB.Descricao = produto.Descricao;

            await db.SaveChangesAsync();
            return Results.Ok(produtoDB);
        }).WithTags("Produtos");

        app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);
            if (produto is null)
            {
                return Results.NotFound();
            }

            db.Produtos.Remove(produto);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Produtos");

    }
}
