using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Guytp.WebApi
{
    /// <summary>
    /// This Swagger operation filter adds support for the Authorization header being required on a particular mtehod.
    /// </summary>
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>();

            bool hasAuthoriseAttributeApplied = authAttributes.Any();
            if (hasAuthoriseAttributeApplied)
            {
                IDictionary<string, IEnumerable<string>> dict = new Dictionary<string, IEnumerable<string>>();
                dict.Add("Bearer", new string[0]);
                if (operation.Security == null)
                    operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                operation.Security.Add(dict);
            }
        }
    }
}