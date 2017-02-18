using System;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Xunit;

namespace Skimia.Extensions.Discovery.Attributes.Tests
{
    public class MethodHandlerTests
    {
        private class TestInstanceHandler
        {
            public bool MethodCalled { get; private set; }

            [TestMethodHandler]
            public void RegisterTestHandler(object token)
            {
                MethodCalled = true;
            }
        }

        private static class TestStaticHandler
        {
            public static bool MethodCalled { get; private set; }

            [TestMethodHandler]
            public static void RegisterTestHandler(object token)
            {
                MethodCalled = true;
            }
        }

        private static class TestAttributeDiscoveredHandler
        {
            public static IMethodHandler Handler { get; private set; }
            [MethodAttributeDiscoveredHandler(typeof(TestMethodHandlerAttribute))]
            public static void RegisterTestHandler(IMethodHandler handler, TestMethodHandlerAttribute handlerAttribute)
            {
                Handler = handler;
                var handlerDelegate = (Action<object, object>) handler.CreateDelegate(typeof(object));

                handlerDelegate(handler.Container, handler);
            }
        }

        [Fact]
        public void MethodHandlerCanCallStaticMethod()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Act
            attributeDiscover.RegisterStaticContainer(typeof(TestStaticHandler));

            // Assert
            Assert.Equal(true, TestStaticHandler.MethodCalled);
        }

        [Fact]
        public void MethodHandlerShouldContainCorrectData()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Act
            attributeDiscover.RegisterStaticContainer(typeof(TestStaticHandler));

            // Assert
            Assert.Equal(typeof(TestStaticHandler), TestAttributeDiscoveredHandler.Handler.ContainerType);
            Assert.Equal(typeof(TestMethodHandlerAttribute), TestAttributeDiscoveredHandler.Handler.HandlerAttribute.GetType());
            Assert.Null(TestAttributeDiscoveredHandler.Handler.Container);
        }

        [Fact]
        public void MethodHandlerCanCallInstanceMethod()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var instanceHandler = new TestInstanceHandler();

            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Act
            attributeDiscover.RegisterInstanceContainer(instanceHandler);

            // Assert
            Assert.Equal(true, instanceHandler.MethodCalled);
        }
    }
}
