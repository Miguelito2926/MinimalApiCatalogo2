using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo2.Context;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

