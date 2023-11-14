using MinimalApiCatalogo2.ApiEndpoints;
using MinimalApiCatalogo2.AppServicesExtensions;

// Criando a inst�ncia do construtor da aplica��o web
var builder = WebApplication.CreateBuilder(args);

// Adicionando Swagger � aplica��o
builder.AddApiSwagger();

// Adicionando persist�ncia (banco de dados) � aplica��o
builder.AddPersistence();

// Adicionando suporte a CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors();

// Adicionando autentica��o e autoriza��o JWT � aplica��o
builder.AddAutenticationJwt();

// Construindo a aplica��o web
var app = builder.Build();

// Mapeando os endpoints relacionados � autentica��o, categorias e produtos
app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

// Obtendo o ambiente da aplica��o
var environment = app.Environment;

// Configurando o tratamento de exce��es, Swagger e CORS
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

// Ativando o servi�o de autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Iniciando a execu��o da aplica��o
app.Run();
