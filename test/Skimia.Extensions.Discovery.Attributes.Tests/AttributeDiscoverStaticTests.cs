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
            public static void RegisterTestMethodHandler() { }
        }

        private class InvalidTestClassAttributeDiscoveredHandler
        {
            [ClassAttributeDiscoveredHandler(typeof(TestClassHandlerAttribute))]
            public static void RegisterTestClassHandler() { }
        }

        private class InvalidTestPropertyAttributeDiscoveredHandler
        {
            [PropertyAttributeDiscoveredHandler(typeof(TestPropertyHandlerAttribute))]
            public static void RegisterTestPropertyHandler() { }
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
        public void ClassAttributeDiscoverShouldBeFailIfAnIncorectHandlerIsRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            Exception ex = Assert.Throws<ArgumentException>(
                    () => attributeDiscover.RegisterStaticContainer(typeof(InvalidTestClassAttributeDiscoveredHandler))
                );
            ;

            // Assert
            Assert.Equal("Class handler Void RegisterTestClassHandler() has incorrect parameters. Right definition is Handler(IClassHandler, ClassHandlerAttribute)", ex.Message);
        }

        [Fact]
        public void PropertyAttributeDiscoverShouldBeFailIfAnIncorectHandlerIsRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            Exception ex = Assert.Throws<ArgumentException>(
                    () => attributeDiscover.RegisterStaticContainer(typeof(InvalidTestPropertyAttributeDiscoveredHandler))
                );
            ;

            // Assert
            Assert.Equal("Property handler Void RegisterTestPropertyHandler() has incorrect parameters. Right definition is Handler(IPropertyHandler, PropertyHandlerAttribute)", ex.Message);
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterAttributeDiscoveredHandler()
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
        public void AttributeDiscoverShouldGiveAttributeDiscoveredHandlerWithCorrectData()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            attributeDiscover.RegisterStaticContainer(typeof(TestAttributeBeforeDiscoveredHandler));

            // Act  
            var methodHandler = attributeDiscover.GetMethodAttributeDiscoveredHandlers(typeof(TestMethodHandlerAttribute)).First();
            var classHandler = attributeDiscover.GetClassAttributeDiscoveredHandlers(typeof(TestClassHandlerAttribute)).First();
            var propertyHandler = attributeDiscover.GetPropertyAttributeDiscoveredHandlers(typeof(TestPropertyHandlerAttribute)).First();

            // Assert
            Assert.Equal(typeof(TestAttributeBeforeDiscoveredHandler), methodHandler.ContainerType);
            Assert.Equal(typeof(TestAttributeBeforeDiscoveredHandler), classHandler.ContainerType);
            Assert.Equal(typeof(TestAttributeBeforeDiscoveredHandler), propertyHandler.ContainerType);
            Assert.Equal(typeof(TestMethodHandlerAttribute), methodHandler.HandlerAttributeType);
            Assert.Equal(typeof(TestClassHandlerAttribute), classHandler.HandlerAttributeType);
            Assert.Equal(typeof(TestPropertyHandlerAttribute), propertyHandler.HandlerAttributeType);
            Assert.Equal(typeof(TestMethodHandlerAttribute), methodHandler.HandlerAttribute.HandlerAttributeType);
            Assert.Equal(typeof(TestClassHandlerAttribute), classHandler.HandlerAttribute.HandlerAttributeType);
            Assert.Equal(typeof(TestPropertyHandlerAttribute), propertyHandler.HandlerAttribute.HandlerAttributeType);
            Assert.Equal(false, methodHandler.HandlerAttribute.HandleAlreadyDiscoveredAttributes);
            Assert.Equal(false, classHandler.HandlerAttribute.HandleAlreadyDiscoveredAttributes);
            Assert.Equal(false, propertyHandler.HandlerAttribute.HandleAlreadyDiscoveredAttributes);
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
