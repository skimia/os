using System;
using System.Linq;
using Xunit;
using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes.Tests
{
    public class AttributeDiscoverStaticTests
    {
        private class InvalidTestMethodAttributeDiscoveredHandler
        {
            [MethodAttributeDiscoveredHandler(typeof(TestMethodHandlerAttribute))]
            public static void RegisterTestMethodHandler() {}
        }

        private class TestAttributeDiscoveredHandler
        {
            public static bool RegisterMethodCalled { get; private set; }
            public static bool RegisterClassCalled { get; private set; }
            public static bool RegisterPropertyCalled { get; private set; }

            [MethodAttributeDiscoveredHandler(typeof(TestMethodHandlerAttribute))]
            public static void RegisterTestMethodHandler(IMethodHandler handler, TestMethodHandlerAttribute handlerAttribute)
            {
                RegisterMethodCalled = true;
            }

            [ClassAttributeDiscoveredHandler(typeof(TestClassHandlerAttribute))]
            public static void RegisterTestClassHandler(IClassHandler handler, TestClassHandlerAttribute handlerAttribute)
            {
                RegisterClassCalled = true;
            }

            [PropertyAttributeDiscoveredHandler(typeof(TestPropertyHandlerAttribute))]
            public static void RegisterTestPropertyHandler(IPropertyHandler handler, TestPropertyHandlerAttribute handlerAttribute)
            {
                RegisterPropertyCalled = true;
            }
        }

        private class TestAttributeBeforeDiscoveredHandler
        {
            public static bool RegisterMethodCalled { get; private set; }
            public static bool RegisterClassCalled { get; private set; }
            public static bool RegisterPropertyCalled => _called == 2;

            private static int _called = 0;

            [MethodAttributeDiscoveredHandler(typeof(TestMethodHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public static void RegisterTestMethodHandler(IMethodHandler handler, TestMethodHandlerAttribute handlerAttribute)
            {
                RegisterMethodCalled = true;
            }

            [ClassAttributeDiscoveredHandler(typeof(TestClassHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public static void RegisterTestClassHandler(IClassHandler handler, TestClassHandlerAttribute handlerAttribute)
            {
                RegisterClassCalled = true;
            }

            [PropertyAttributeDiscoveredHandler(typeof(TestPropertyHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public static void RegisterTestPropertyHandler(IPropertyHandler handler, TestPropertyHandlerAttribute handlerAttribute)
            {
                _called++;
            }
        }

        [TestClassHandler]
        private class TestHandler
        {
            [TestMethodHandler]
            public static void RegisterTestHandler(object token) {}

            [TestPropertyHandler]
            public static string TestProperty { get; set; }

            [TestPropertyHandler]
            public static int _test = 5;
        }

        [Fact]
        public void MethodAttributeDiscoverShouldBeFailIfAnIncorectHandlerIsRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            Exception ex = Assert.Throws<ArgumentException>(
                    () => attributeDiscover.RegisterStaticContainer(typeof(InvalidTestMethodAttributeDiscoveredHandler))
                );
            ;

            // Assert
            Assert.Equal("Method handler Void RegisterTestMethodHandler() has incorrect parameters. Right definition is Handler(IMethodHandler, MethodHandlerAttribute)", ex.Message);
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterMethodAttributeDiscoveredHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Assert
            Assert.Equal(1, attributeDiscover.GetMethodAttributeDiscoveredHandlers(typeof(TestMethodHandlerAttribute)).Count());
            Assert.Equal(1, attributeDiscover.GetClassAttributeDiscoveredHandlers(typeof(TestClassHandlerAttribute)).Count());
            Assert.Equal(1, attributeDiscover.GetPropertyAttributeDiscoveredHandlers(typeof(TestPropertyHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Assert
            Assert.Equal(1, attributeDiscover.GetMethodHandlers(typeof(TestMethodHandlerAttribute)).Count());
            Assert.Equal(1, attributeDiscover.GetClassHandlers(typeof(TestClassHandlerAttribute)).Count());
            Assert.Equal(2, attributeDiscover.GetPropertyHandlers(typeof(TestPropertyHandlerAttribute)).Count());
        }

        [Fact]
        public void MethodAttributeDiscoverShouldBeCalledOnNewStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Assert
            Assert.True(TestAttributeDiscoveredHandler.RegisterMethodCalled);
            Assert.True(TestAttributeDiscoveredHandler.RegisterClassCalled);
            Assert.True(TestAttributeDiscoveredHandler.RegisterPropertyCalled);
        }

        [Fact]
        public void MethodAttributeDiscoverShouldBeCalledAfterStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeDiscoveredHandler));

            // Assert
            Assert.True(TestAttributeDiscoveredHandler.RegisterMethodCalled);
            Assert.True(TestAttributeDiscoveredHandler.RegisterClassCalled);
            Assert.True(TestAttributeDiscoveredHandler.RegisterPropertyCalled);
        }

        [Fact]
        public void MethodeAttributeDiscoverShouldNotBeCalledAfterStaticTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterStaticContainer(typeof(TestHandler));

            // Act  
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeBeforeDiscoveredHandler));

            // Assert
            Assert.False(TestAttributeBeforeDiscoveredHandler.RegisterMethodCalled);
            Assert.False(TestAttributeBeforeDiscoveredHandler.RegisterClassCalled);
            Assert.False(TestAttributeBeforeDiscoveredHandler.RegisterPropertyCalled);
        }

    }
}
