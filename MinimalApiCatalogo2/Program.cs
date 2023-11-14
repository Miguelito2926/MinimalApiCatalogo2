using MinimalApiCatalogo2.ApiEndpoints;
using MinimalApiCatalogo2.AppServicesExtensions;

// Criando a instância do construtor da aplicação web
var builder = WebApplication.CreateBuilder(args);

// Adicionando Swagger à aplicação
builder.AddApiSwagger();

// Adicionando persistência (banco de dados) à aplicação
builder.AddPersistence();

// Adicionando suporte a CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors();

// Adicionando autenticação e autorização JWT à aplicação
builder.AddAutenticationJwt();

// Construindo a aplicação web
var app = builder.Build();

// Mapeando os endpoints relacionados à autenticação, categorias e produtos
app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

// Obtendo o ambiente da aplicação
var environment = app.Environment;

// Configurando o tratamento de exceções, Swagger e CORS
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

// Ativando o serviço de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Iniciando a execução da aplicação
app.Run();
