
using MinimalApiCatalogo2.ApiEndpoints;
using MinimalApiCatalogo2.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();


//Ativando o serviço de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();
app.Run();

