/*
 * for ?? operator show https://msdn.microsoft.com/fr-fr/library/ms173224.aspx
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLCore.Type;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.GraphQl.Attributes;

namespace Skimia.Extensions.GraphQl.Schema
{
    public class RootSchema : GraphQLSchema
    {
        private Query _rootQuery;
        public RootSchema(IAttributeDiscover discover, Query rootQuery)
        {
            _rootQuery = rootQuery;
            this.AddKnownType(_rootQuery);
            this.Query(_rootQuery);

            //should use better implementation
            discover.RegisterInstanceContainer(this);
        }

        [ClassAttributeDiscoveredHandler(typeof(GraphQlTypeAttribute))]
        public void RegisterTestClassHandler(IClassHandler handler, GraphQlTypeAttribute graphQlAttribute)
        {
            AddKnownType( (GraphQLBaseType)(handler.Container ?? Activator.CreateInstance(handler.ContainerType, _rootQuery)) );
        }
    }
}
