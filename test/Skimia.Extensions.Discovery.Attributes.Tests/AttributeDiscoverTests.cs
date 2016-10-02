using Skimia.Extensions.Discovery.Attributes.Abstractions;
using System;
using System.Linq;
using Xunit;

namespace Skimia.Extensions.Discovery.Attributes.Tests
{
    public class AttributeDiscoverTests
    {
        private class InvalidTestAttributeDiscoveredHandler
        {
            [AttributeDiscoveredHandler(typeof(TestHandlerAttribute))]
            public static void RegisterTestHandler()
            {

            }
        }

        private class TestAttributeDiscoveredHandler
        {
            public static bool RegisterCalled { get; private set; }

            [AttributeDiscoveredHandler(typeof(TestHandlerAttribute))]
            public static void RegisterTestHandler(IHandler handler, TestHandlerAttribute handlerAttribute)
            {
                RegisterCalled = true;
            }
        }

        private class TestAttributeBeforeDiscoveredHandler
        {
            public static bool RegisterCalled { get; private set; }

            [AttributeDiscoveredHandler(typeof(TestHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public static void RegisterTestHandler(IHandler handler, TestHandlerAttribute handlerAttribute)
            {
                RegisterCalled = true;
            }
        }

        private class TestHandler
        {
            [TestHandler]
            public static void RegisterTestHandler(object token)
            {

            }
        }

        private class TestAttributeDiscoveredInstanceHandler
        {
            public bool RegisterCalled { get; private set; }

            [AttributeDiscoveredHandler(typeof(TestHandlerAttribute))]
            public void RegisterTestHandler(IHandler handler, TestHandlerAttribute handlerAttribute)
            {
                RegisterCalled = true;
            }
        }

        private class TestInstanceHandler
        {
            [TestHandler]
            public void RegisterTestHandler(object token)
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
                    () => attributeDiscover.RegisterStaticContainer(typeof(InvalidTestAttributeDiscoveredHandler))
                );
            ;

            // Assert
            Assert.Equal("Method handler Void RegisterTestHandler() has incorrect parameters. Right definition is Handler(IHandler, HandlerAttribute)", ex.Message);
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterAttributeDiscoveredHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Assert
            Assert.Equal(1, attributeDiscover.GetAttributeDiscoveredHandlers(typeof(TestHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Assert
            Assert.Equal(1, attributeDiscover.GetHandlers(typeof(TestHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldBeCalledOnNewStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Assert
            Assert.True(TestAttributeDiscoveredHandler.RegisterCalled);
        }

        [Fact]
        public void AttributeDiscoverShouldBeCalledAfterStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Assert
            Assert.True(TestAttributeDiscoveredHandler.RegisterCalled);
        }

        [Fact]
        public void AttributeDiscoverShouldNotBeCalledAfterStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeBeforeDiscoveredHandler));

            // Assert
            Assert.False(TestAttributeBeforeDiscoveredHandler.RegisterCalled);
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterAttributeDiscoveredInstanceHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var attributeDiscoverInstanceHandler = new TestAttributeDiscoveredInstanceHandler();
            // Act  
            attributeDiscover.RegisterInstanceContainer(attributeDiscoverInstanceHandler);

            // Assert
            Assert.Equal(1, attributeDiscover.GetAttributeDiscoveredHandlers(typeof(TestHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterInstanceHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var instanceHandler = new TestInstanceHandler();

            // Act  
            attributeDiscover.RegisterInstanceContainer(instanceHandler);

            // Assert
            Assert.Equal(1, attributeDiscover.GetHandlers(typeof(TestHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldBeCalledOnNewStaticInstanceTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var attributeDiscoverInstanceHandler = new TestAttributeDiscoveredInstanceHandler();
            var instanceHandler = new TestInstanceHandler();

            attributeDiscover.RegisterInstanceContainer(attributeDiscoverInstanceHandler);

            // Act  
            attributeDiscover.RegisterInstanceContainer(instanceHandler);

            // Assert
            Assert.True(attributeDiscoverInstanceHandler.RegisterCalled);
        }

    }
}
