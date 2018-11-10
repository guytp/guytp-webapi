using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Guytp.WebApi
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            AppConfig appConfig = AppConfig.ApplicationInstance;
            services.AddCors();
            services.AddRouting();

            if (AppConfig.ApplicationInstance.JwtAuth.IsMiddlewareEnabled)
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = appConfig.JwtAuth.Audience;
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    X509SecurityKey key = null;
                    if (!string.IsNullOrEmpty(appConfig.JwtAuth.CertificateData))
                        key = new X509SecurityKey(new X509Certificate2(Convert.FromBase64String(appConfig.JwtAuth.CertificateData)));
                    else
                        key = new X509SecurityKey(GetCertificateFromStore(appConfig.JwtAuth.CertificateStoreLocation, appConfig.JwtAuth.CertificateDistinguishedName));
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = appConfig.JwtAuth.Issuer,
                        RequireSignedTokens = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromMinutes(2),
                        ValidAudience = appConfig.JwtAuth.Audience,
                        IssuerSigningKey = key
                    };
                });

            services.AddMvc()
              .AddApplicationPart(Assembly.GetEntryAssembly())
              .AddControllersAsServices()
              .AddJsonOptions(options =>
              {
                  (options.SerializerSettings.ContractResolver as DefaultContractResolver).IgnoreSerializableAttribute = true;
                  options.SerializerSettings.Formatting = Formatting.None;
                  options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                  if (appConfig?.Api == null || appConfig.Api.IsEnumSerialisedAsString)
                      options.SerializerSettings.Converters.Add(new StringEnumConverter());
              }).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
            SwaggerSettings swaggerConfig = appConfig?.Swagger;
            if (swaggerConfig != null && swaggerConfig.IsEnabled)
                services.AddSwaggerGen(c =>
                {
                    c.OperationFilter<AuthResponsesOperationFilter>();
                    c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
                    c.EnableAnnotations();
                    var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute));
                    AssemblyTitleAttribute assemblyTitleAttribute = attributes.SingleOrDefault() as AssemblyTitleAttribute;
                    c.SwaggerDoc("v" + appConfig.Api.Version, new Swashbuckle.AspNetCore.Swagger.Info { Title = assemblyTitleAttribute?.Title, Version = "v" + appConfig.Api.Version });
                    string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)) + ".xml";
                    c.IncludeXmlComments(filePath);
                    if (appConfig.JwtAuth != null && appConfig.JwtAuth.IsMiddlewareEnabled)
                        c.AddSecurityDefinition("Bearer", new JwtAuthScheme());
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            AppConfig appConfig = AppConfig.ApplicationInstance;

            if (AppConfig.ApplicationInstance.JwtAuth.IsMiddlewareEnabled)
                app.UseAuthentication();

            ExceptionHandlingSettings exceptionConfig = appConfig?.ExceptionHandling;
            if (exceptionConfig != null && exceptionConfig.IsMiddlewareEnabled)
                app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

            // If configured to do so, setup Swagger based on our configuration
            SwaggerSettings swaggerConfig = appConfig?.Swagger;
            if (swaggerConfig != null && swaggerConfig.IsEnabled)
            {
                app.UseStaticFiles();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = swaggerConfig.SwaggerRoute + "/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = swaggerConfig.SwaggerUiRoute;
                    var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute));
                    AssemblyTitleAttribute assemblyTitleAttribute = attributes.SingleOrDefault() as AssemblyTitleAttribute;
                    string title = assemblyTitleAttribute?.Title + " v" + appConfig.Api.Version;
                    c.SwaggerEndpoint((string.IsNullOrEmpty(swaggerConfig.OverrideRootUrl) ? string.Empty : swaggerConfig.OverrideRootUrl) + "/" + swaggerConfig.SwaggerRoute + "/v" + appConfig.Api.Version + "/swagger.json", title);
                });
            }

            // Add CORS settings
            if (appConfig.Api == null || appConfig.Api.IsCorsAllowingAll)
                app.UseCors(
                    o =>
                    {
                        o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });

            app.UseMvc();
        }

        #region Helper Methods
        /// <summary>
        /// Obtain a certificate from the certificate store.
        /// </summary>
        /// <param name="storeLocation">
        /// The store location to use for the certificate lookup.
        /// </param>
        /// <param name="subjectDistinguishedName">
        /// The certificate to obtain's distinguished name.
        /// </param>
        /// <param name="isTrustedRoot">
        /// Whether or not this certifcate has a trusted root.
        /// </param>
        /// <returns></returns>
        private static X509Certificate2 GetCertificateFromStore(StoreLocation storeLocation, string subjectDistinguishedName, bool isTrustedRoot = false)
        {
            // Get the correct certificate store
            X509Store store = new X509Store(storeLocation);
            try
            {
                // Open the store read-only
                store.Open(OpenFlags.ReadOnly);

                // Find the certificate based on its DN
                X509Certificate2Collection certCollection = store.Certificates;
                X509Certificate2Collection signingCert;
                if (isTrustedRoot)
                    // If trusted we can lookup directly
                    signingCert = certCollection.Find(X509FindType.FindBySubjectDistinguishedName, subjectDistinguishedName, true);
                else
                {
                    // If not trusted then we need to lookup all valid first
                    X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                    signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, subjectDistinguishedName, false);
                }

                // Return found certificate
                return signingCert.Count == 0 ? null : signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }
        #endregion
    }
}
