using GraphQLCore.Type;
using GraphQLCore.Type.Complex;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.GraphQl;
using Skimia.Extensions.GraphQl.Attributes;

namespace Skimia.Plugins.FrontConfig.Schema.Result
{
    [GraphQlType]
    public class FrontState : GraphQLObjectType<Configuration.Front.StateOptions>
    {
        public FrontState() : base("FrontState", "represents a state configured by a plugin")
        {
            Field("name", e => e.Name);
            Field("assets", e => e.Assets);
            Field("url", e => e.Url);
            Field("views", e => e.Views);
        }

        public static bool HasField(string fieldName)
        {
            var frontConfigGraphObject = GraphQl.ServiceProvider.GetService<FrontState>();

            return frontConfigGraphObject.Fields.ContainsKey(fieldName);
        }

        public static bool HasFieldValue(string fieldName, Configuration.Front.StateOptions objectConfiguration)
        {
            var frontConfigGraphObject = GraphQl.ServiceProvider.GetService<FrontState>();

            GraphQLObjectTypeFieldInfo value;
            if (!frontConfigGraphObject.Fields.TryGetValue(fieldName, out value))
            {
                return false;
            }

            return value.Lambda.Compile().DynamicInvoke(objectConfiguration) != null;
        }
    }
}
