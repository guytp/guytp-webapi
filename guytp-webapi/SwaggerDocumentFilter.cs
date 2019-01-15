using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Guytp.WebApi
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            SwaggerSettings swaggerConfig = AppConfig.ApplicationInstance.Swagger;
            swaggerDoc.Host = swaggerConfig.Host;
            swaggerDoc.BasePath = swaggerConfig.BasePath;
            swaggerDoc.Schemes = new List<string> { swaggerConfig.Scheme };
        }
    }
}