using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MinimalApiCatalogo2.Context;

// Cria��o do construtor da aplica��o web
var builder = WebApplication.CreateBuilder(args);

// Adi��o de servi�os ao cont�iner de inje��o de depend�ncia
// Configura��o do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obten��o da string de conex�o do arquivo de configura��o
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adi��o do contexto do banco de dados ao servi�o de inje��o de depend�ncia
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, ServerVersion(connectionString)));

// M�todo para configurar a vers�o do servidor SQL
Action<SqlServerDbContextOptionsBuilder>? ServerVersion(string? connectionString)
{
    // Implementa��o n�o fornecida (deve ser definida conforme necess�rio)
    throw new NotImplementedException("Erro de vers�o do Database.");
}

// Constru��o da aplica��o
var app = builder.Build();

// Configura��o do pipeline de solicita��o HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Execu��o da aplica��o
app.Run();
