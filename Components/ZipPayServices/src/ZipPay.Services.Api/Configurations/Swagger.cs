using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ZipPay.Services.Api.Configurations
{
    public static class Swagger
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "User Service Api", Version = "v1" });
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                });
        }

        public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder app)
        {
            return app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "User Service Api V1");
            });
        }
    }
}
