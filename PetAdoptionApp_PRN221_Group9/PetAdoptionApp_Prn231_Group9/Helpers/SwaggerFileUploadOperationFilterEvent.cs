using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetAdoptionApp_Prn231_Group9.Helpers
{
    public class SwaggerFileUploadOperationFilterEvent : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile));

            if (fileParams.Any())
            {
                // Clear existing parameters
                operation.Parameters.Clear();

                // Add 'petId' as a path parameter
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "eventId",
                    In = ParameterLocation.Path, // Change this to Path
                    Required = true,
                    Description = "The ID of the pet to associate the image with.",
                    Schema = new OpenApiSchema
                    {
                        Type = "string", // Guid will be represented as a string in URL
                        Format = "uuid" // Optional: Specify the format
                    }
                });

                // Define the request body for file upload
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["file"] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                },
                                Required = new HashSet<string> { "file" } // Only 'file' is required
                            }
                        }
                    }
                };
            }
        }
    }
}
