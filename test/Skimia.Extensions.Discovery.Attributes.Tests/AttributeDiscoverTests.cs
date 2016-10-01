using Skimia.Extensions.Discovery.Attributes.Abstractions;
using System;
using System.Linq;
using Xunit;

namespace Skimia.Extensions.Discovery.Attributes.Tests
{
    public class AttributeDiscoverTests
    {
        protected class InvalidTestAttributeDiscoveredHandler
        {
            [AttributeDiscoveredHandler(typeof(TestHandlerAttribute))]
            public static void RegisterTestHandler()
            {

            }
        }

        protected class TestAttributeDiscoveredHandler
        {
            public static bool RegisterCalled { get; private set; }

            [AttributeDiscoveredHandler(typeof(TestHandlerAttribute))]
            public static void RegisterTestHandler(IHandler handler, TestHandlerAttribute handlerAttribute)
            {
                RegisterCalled = true;
            }
        }

        protected class TestAttributeBeforeDiscoveredHandler
        {
            public static bool RegisterCalled { get; private set; }

            [AttributeDiscoveredHandler(typeof(TestHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public static void RegisterTestHandler(IHandler handler, TestHandlerAttribute handlerAttribute)
            {
                RegisterCalled = true;
            }
        }

        protected class TestHandler
        {
            [TestHandler]
            public static void RegisterTestHandler(object token)
            {

            }
        }

        [Fact]
        public void AttributeDiscoverShouldBeFailIfAnIncorectHandlerIsRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            Exception ex = Assert.Throws<ArgumentException>(
                    () => attributeDiscover.RegisterSharedStaticContainer(typeof(InvalidTestAttributeDiscoveredHandler))
                );
            ;

            // Assert
            Assert.Equal("Method handler Void RegisterTestHandler() has incorrect parameters. Right definition is Handler(IHandler, HandlerAttribute)", ex.Message);
        }

        [Fact]
        public void AttributeDiscoverShouldBeRegisterAttributeDiscoveredHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Assert
            Assert.Equal(1, attributeDiscover.GetAttributeDiscoveredHandlers(typeof(TestHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldBeRegisterHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestHandler));

            // Assert
            Assert.Equal(1, attributeDiscover.GetHandlers(typeof(TestHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldBeCalledOnNewStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Act  
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestHandler));

            // Assert
            Assert.True(TestAttributeDiscoveredHandler.RegisterCalled);
        }

        [Fact]
        public void AttributeDiscoverShouldBeCalledAfterStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestHandler));

            // Act  
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Assert
            Assert.True(TestAttributeDiscoveredHandler.RegisterCalled);
        }

        [Fact]
        public void AttributeDiscoverShouldNotBeCalledAfterStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestHandler));

            // Act  
            attributeDiscover.RegisterSharedStaticContainer(typeof(TestAttributeBeforeDiscoveredHandler));

            // Assert
            Assert.False(TestAttributeBeforeDiscoveredHandler.RegisterCalled);
        }

    }
}
