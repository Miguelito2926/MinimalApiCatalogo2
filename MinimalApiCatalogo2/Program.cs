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
/*Desativar a Verifica��o SSL (N�o Recomendado): Esta � uma op��o que voc� pode considerar apenas
em ambientes de desenvolvimento e nunca em produ��o. Voc� pode desativar a verifica��o SSL 
adicionando "TrustServerCertificate=true" � sua string de conex�o. No entanto, isso torna a conex�o menos segura.*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

