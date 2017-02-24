using GraphQLCore.Type;
using GraphQLCore.Type.Complex;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.GraphQl;
using Skimia.Extensions.GraphQl.Attributes;

namespace Skimia.Plugins.FrontConfig.Schema.Result
{
    [GraphQlType]
    public class FrontView : GraphQLObjectType<Configuration.Front.ViewOption>
    {
        public FrontView() : base("FrontView", "represents a view implemented by a plugin")
        {
            Field("name", e => e.InjectInView);
            Field("controller", e => e.Controller);
            Field("templateUrl", e => e.TemplateUrl);
        }

        public static bool HasField(string fieldName)
        {
            var frontConfigGraphObject = GraphQl.ServiceProvider.GetService<FrontView>();

            return frontConfigGraphObject.Fields.ContainsKey(fieldName);
        }

        public static bool HasFieldValue(string fieldName, Configuration.Front.ViewOption objectConfiguration)
        {
            var frontConfigGraphObject = GraphQl.ServiceProvider.GetService<FrontView>();

            GraphQLObjectTypeFieldInfo value;
            if (!frontConfigGraphObject.Fields.TryGetValue(fieldName, out value))
            {
                return false;
            }

            return value.Lambda.Compile().DynamicInvoke(objectConfiguration) != null;
        }
    }
}
