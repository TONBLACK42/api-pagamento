using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace tech_test_payment_api.Payment.Api.Models
{
    public class AddSchemaExample : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = GetExampleOrNullFor(context.Type);
        }

        private IOpenApiAny GetExampleOrNullFor(Type type)
        {
            switch (type.Name)
            {
                case "Operation":
                    return new OpenApiObject
                    {
                        [ "op" ] = new OpenApiString("Replace"),
                        [ "path" ] = new OpenApiString("/status"),
                        [ "value" ] = new OpenApiInteger(2)
                    };
                default:
                    return null;
            }
        }
    }
}