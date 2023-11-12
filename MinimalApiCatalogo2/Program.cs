using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo2.Context;
using MinimalApiCatalogo2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuring Swagger/OpenAPI  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString: "Data Source=PRILLNOTEBOOK28\\SQLEXPRESS;Initial Catalog=MinimalApiCatalogoDB;Integrated Security=True;TrustServerCertificate=true"));
/*Desativar a Verificação SSL (Não Recomendado): Esta é uma opção que você pode considerar apenas
em ambientes de desenvolvimento e nunca em produção. Você pode desativar a verificação SSL 
adicionando "TrustServerCertificate=true" à sua string de conexão. No entanto, isso torna a conexão menos segura.*/

var app = builder.Build();

app.MapGet("/", () => "Catálogo de Produtos - 2023").ExcludeFromDescription();

app.MapGet("/categorias", async(AppDbContext db) => await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id)
       is Categoria categoria
       ? Results.Ok(categoria)
       : Results.NotFound();
});

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

app.MapPut("/categorias/{id:int}", async (int id,Categoria categoria, AppDbContext db) =>
{
    if(categoria.CategoriaId != id)
    {
        return Results.BadRequest();
    }
    var categoriaDB = await db.Categorias.FindAsync(id);
    if(categoriaDB is null)return Results.NotFound();
    
    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
});

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if(categoria is null)
    {
        return Results.NotFound();
    }

    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//---------------ENDPOINTS PARA PRODUTOS---------------------
app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync());

app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
{ 
    return  await db.Produtos.FindAsync(id)
         is Produto produto
        ?Results.Ok(produto) 
        : Results.NotFound();
});

app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{produto.ProdutoId}", produto);
});

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
});

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
});

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
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

