/*
 * for ?? operator show https://msdn.microsoft.com/fr-fr/library/ms173224.aspx
 */
using System;
using GraphQLCore.Type;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.GraphQl.Attributes;

namespace Skimia.Extensions.GraphQl.Schema
{
    public class RootSchema : GraphQLSchema
    {
        private IServiceProvider _serviceProvider;
        public RootSchema(IAttributeDiscover discover, Query rootQuery, Mutation rootMutation, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this.AddKnownType(rootQuery);
            this.AddKnownType(rootMutation);
            this.Query(rootQuery);
            this.Mutation(rootMutation);

            //should use better implementation
            discover.RegisterInstanceContainer(this);
        }

        [ClassAttributeDiscoveredHandler(typeof(GraphQlTypeAttribute))]
        public void RegisterTypeHandler(IClassHandler handler, GraphQlTypeAttribute graphQlAttribute)
        {
            AddKnownType( (GraphQLBaseType)_serviceProvider.GetRequiredService(handler.ContainerType) );
        }
    }
}
