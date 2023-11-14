// Importando namespaces necessários
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApiCatalogo2.Context;
using MinimalApiCatalogo2.Services;
using System.Text;

namespace MinimalApiCatalogo2.AppServicesExtensions
{
    public static class ServiceCollectionExtensions
    {
        // Método de extensão para adicionar Swagger à aplicação
        public static WebApplicationBuilder AddApiSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwagger();
            return builder;
        }

        // Método de extensão para configurar Swagger na coleção de serviços
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Configuração da documentação do Swagger
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinimalApiCatalogo", Version = "v1" });

                // Configuração da segurança para JWT no Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme.
                             Enter 'Bearer'[space].Example: \'Bearer 12345abcdef\'",
                });

                // Configuração de requisitos de segurança para JWT no Swagger
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return services;
        }

        // Método de extensão para adicionar persistência (banco de dados)
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            // Obtendo a string de conexão do arquivo de configuração
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Configurando o contexto do banco de dados com o provedor SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registrando o serviço Jwt na coleção de serviços
            builder.Services.AddSingleton<ITokenService>(new TokenService());
            return builder;
        }

        // Método de extensão para adicionar autenticação JWT
        public static WebApplicationBuilder AddAutenticationJwt(this WebApplicationBuilder builder)
        {
            // Registrando serviços de autenticação e validação de Token JWT
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
                };
            });

            // Adicionando o serviço de autorização
            builder.Services.AddAuthorization();
            return builder;
        }
    }
}
