using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApiCatalogo2.ApiEndpoints;
using MinimalApiCatalogo2.Context;
using MinimalApiCatalogo2.Models;
using MinimalApiCatalogo2.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuring Swagger/OpenAPI  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString: "Data Source=PRILLNOTEBOOK28\\SQLEXPRESS;Initial Catalog=MinimalApiCatalogoDB;Integrated Security=True;TrustServerCertificate=true"));
/*Desativar a Verifica��o SSL (N�o Recomendado): Esta � uma op��o que voc� pode considerar apenas
em ambientes de desenvolvimento e nunca em produ��o. Voc� pode desativar a verifica��o SSL 
adicionando "TrustServerCertificate=true" � sua string de conex�o. No entanto, isso torna a conex�o menos segura.*/

//Registrando Servi�o Jwt
builder.Services.AddSingleton<ITokenService>(new TokenService());

//Registrando servi�os de autentica��es e valida��o de Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();
app.MapAutenticacaoEndpoints();

app.MapGet("/", () => "Cat�logo de Produtos - 2023").ExcludeFromDescription();

app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync()).WithTags("Categorias").RequireAuthorization();

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id)
       is Categoria categoria
       ? Results.Ok(categoria)
       : Results.NotFound();
}).WithTags("Categorias");

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
}).WithTags("Categorias");

app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
{
    if (categoria.CategoriaId != id)
    {
        return Results.BadRequest();
    }
    var categoriaDB = await db.Categorias.FindAsync(id);
    if (categoriaDB is null) return Results.NotFound();

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
}).WithTags("Categorias");

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null)
    {
        return Results.NotFound();
    }

    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Categorias");

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Ativando o servi�o de autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();
app.Run();

