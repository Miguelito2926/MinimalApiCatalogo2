using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MinimalApiCatalogo2.Context;

// Criação do construtor da aplicação web
var builder = WebApplication.CreateBuilder(args);

// Adição de serviços ao contêiner de injeção de dependência
// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtenção da string de conexão do arquivo de configuração
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adição do contexto do banco de dados ao serviço de injeção de dependência
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, ServerVersion(connectionString)));

// Método para configurar a versão do servidor SQL
Action<SqlServerDbContextOptionsBuilder>? ServerVersion(string? connectionString)
{
    // Implementação não fornecida (deve ser definida conforme necessário)
    throw new NotImplementedException("Erro de versão do Database.");
}

// Construção da aplicação
var app = builder.Build();

// Configuração do pipeline de solicitação HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Execução da aplicação
app.Run();
