using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Skimia.Extensions.Discovery.Attributes.Tests
{
    public class AttributeDiscoverAssembliesTests
    {
        [Fact]
        public void RegisterAssembliesShouldBeDiscoverAllStaticHandlers()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var assembliesList = new List<Assembly>()
            {
                Assembly.GetEntryAssembly()
            };

            // Act  
            attributeDiscover.RegisterAssemblies(assembliesList);

            // Assert
            Assert.Equal(0, attributeDiscover.GetMethodAttributeDiscoveredHandlers(typeof(TestMethodHandlerAttribute)).Count());
            Assert.Equal(0, attributeDiscover.GetClassAttributeDiscoveredHandlers(typeof(TestClassHandlerAttribute)).Count());
            Assert.Equal(0, attributeDiscover.GetPropertyAttributeDiscoveredHandlers(typeof(TestPropertyHandlerAttribute)).Count());
        }
    }
}
