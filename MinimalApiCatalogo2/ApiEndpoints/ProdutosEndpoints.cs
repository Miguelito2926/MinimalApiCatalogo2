// Importando namespaces necessários
using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo2.Context;
using MinimalApiCatalogo2.Models;

namespace MinimalApiCatalogo2.ApiEndpoints
{
    public static class ProdutosEndpoints
    {
        // Método para mapear os endpoints relacionados a produtos
        public static void MapProdutosEndpoints(this WebApplication app)
        {
            // Endpoint para obter todos os produtos
            app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync()).WithTags("Produtos").RequireAuthorization();

            // Endpoint para obter um produto por ID
            app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
            {
                // Tenta encontrar o produto com o ID fornecido no banco de dados
                return await db.Produtos.FindAsync(id)
                    // Se encontrado, retorna 200 OK com o produto
                    is Produto produto
                    ? Results.Ok(produto)
                    // Se não encontrado, retorna 404 Not Found
                    : Results.NotFound();
            }).WithTags("Produtos");

            // Endpoint para adicionar um novo produto
            app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
            {
                // Adiciona o novo produto ao contexto do banco de dados
                db.Produtos.Add(produto);
                // Salva as mudanças no banco de dados
                await db.SaveChangesAsync();
                // Retorna 201 Created com o novo produto e sua localização
                return Results.Created($"/categorias/{produto.ProdutoId}", produto);
            }).WithTags("Produtos");

            // Endpoint para atualizar um produto existente (corrigindo a rota para "produtos/{id:int}")
            app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
            {
                // Verifica se o ID na URL corresponde ao ID do produto
                if (produto.ProdutoId != id)
                {
                    // Se não corresponder, retorna 400 Bad Request
                    return Results.BadRequest();
                }

                // Tenta encontrar o produto com o ID fornecido no banco de dados
                var produtoDB = await db.Produtos.FindAsync(id);
                // Se não encontrado, retorna 404 Not Found
                if (produtoDB is null) return Results.NotFound();

                // Atualiza os dados do produto existente com os novos dados
                produtoDB.Nome = produto.Nome;
                produtoDB.Descricao = produto.Descricao;

                // Salva as mudanças no banco de dados
                await db.SaveChangesAsync();
                // Retorna 200 OK com o produto atualizado
                return Results.Ok(produtoDB);
            }).WithTags("Produtos");

            // Endpoint para excluir um produto
            app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
            {
                // Tenta encontrar o produto com o ID fornecido no banco de dados
                var produto = await db.Produtos.FindAsync(id);
                // Se não encontrado, retorna 404 Not Found
                if (produto is null)
                {
                    return Results.NotFound();
                }

                // Remove o produto do contexto do banco de dados
                db.Produtos.Remove(produto);
                // Salva as mudanças no banco de dados
                await db.SaveChangesAsync();
                // Retorna 204 No Content indicando sucesso sem conteúdo
                return Results.NoContent();
            }).WithTags("Produtos");
        }
    }
}
