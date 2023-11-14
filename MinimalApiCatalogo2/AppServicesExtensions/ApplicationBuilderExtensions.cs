namespace MinimalApiCatalogo2.AppServicesExtensions
{
    public static class ApplicationBuilderExtensions
    {
        // Método para configurar o tratamento de exceções com base no ambiente
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // Se o ambiente for de desenvolvimento, usa a página de exceções do desenvolvedor
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            return app;
        }

        // Método para configurar as políticas CORS da aplicação
        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors(p =>
            {
                // Permite qualquer origem
                p.AllowAnyOrigin();
                // Permite apenas requisições do tipo GET
                p.WithMethods("GET");
                // Permite qualquer cabeçalho
                p.AllowAnyHeader();
            });
            return app;
        }

        // Método para adicionar middleware do Swagger
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            // Habilita o uso do Swagger
            app.UseSwagger();
            // Configura a interface do usuário do Swagger (neste caso, sem configurações adicionais)
            app.UseSwaggerUI(c => { });
            return app;
        }
    }
}
