using Swashbuckle.AspNetCore.Swagger;

namespace Guytp.WebApi
{
    public class JwtAuthScheme : SecurityScheme
    {
        public JwtAuthScheme()
        {
            Type = "apiKey";
            Extensions.Add("name", "Authorization");
            Extensions.Add("in", "header");
        }
    }
}
