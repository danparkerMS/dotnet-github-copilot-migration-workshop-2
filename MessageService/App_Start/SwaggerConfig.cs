using System.Web.Http;
using WebActivatorEx;
using MessageService;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace MessageService
{
    /// <summary>
    /// Swagger configuration
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Registers Swagger configuration
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "MessageService API")
                        .Description("A simple REST API that returns greeting messages with timestamps");
                    
                    var xmlPath = GetXmlCommentsPath();
                    if (System.IO.File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                })
                .EnableSwaggerUi();
        }

        /// <summary>
        /// Gets the path to the XML documentation file
        /// </summary>
        /// <returns>Path to XML file</returns>
        private static string GetXmlCommentsPath()
        {
            return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin", "MessageService.xml");
        }
    }
}
