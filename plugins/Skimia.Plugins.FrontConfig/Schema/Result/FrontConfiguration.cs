using GraphQLCore.Type;
using GraphQLCore.Type.Complex;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.GraphQl;
using Skimia.Extensions.GraphQl.Attributes;

namespace Skimia.Plugins.FrontConfig.Schema.Result
{
    [GraphQlType]
    public class FrontConfiguration : GraphQLObjectType<Configuration.Front.Configuration>
    {
        public FrontConfiguration() : base("FrontConfiguration", "represents the configuration of a plugin")
        {
            Field("name", e => e.Name);
            Field("otherwise", e => e.Otherwise);
            Field("version", e => e.Version);
            Field("assetsBeforeLoad", e => e.AssetsBeforeLoad);
            Field("routing", e => e.Routing);
        }

        public static bool HasField(string fieldName)
        {
            var frontConfigGraphObject = GraphQl.ServiceProvider.GetService<FrontConfiguration>();

            return frontConfigGraphObject.Fields.ContainsKey(fieldName);
        }

        public static bool HasFieldValue(string fieldName, Configuration.Front.Configuration objectConfiguration)
        {
            var frontConfigGraphObject = GraphQl.ServiceProvider.GetService<FrontConfiguration>();

            GraphQLObjectTypeFieldInfo value;
            if (!frontConfigGraphObject.Fields.TryGetValue(fieldName, out value))
            {
                return false;
            }

            return value.Lambda.Compile().DynamicInvoke(objectConfiguration) != null;
        }
    }
}
