using Skimia.Extensions.Discovery.Attributes.Abstractions;
using System;
using System.Linq;
using Xunit;

namespace Skimia.Extensions.Discovery.Attributes.Tests
{
    public class AttributeDiscoverInstanceTests
    {
        private class InvalidTestMethodAttributeDiscoveredHandler
        {
            [MethodAttributeDiscoveredHandler(typeof(TestMethodHandlerAttribute))]
            public void RegisterTestMethodHandler() { }
        }

        private class TestAttributeDiscoveredHandler
        {
            public bool RegisterMethodCalled { get; private set; }
            public bool RegisterClassCalled { get; private set; }
            public bool RegisterPropertyCalled { get; private set; }

            [MethodAttributeDiscoveredHandler(typeof(TestMethodHandlerAttribute))]
            public void RegisterTestHandler(IMethodHandler handler, TestMethodHandlerAttribute handlerAttribute)
            {
                RegisterMethodCalled = true;
            }

            [ClassAttributeDiscoveredHandler(typeof(TestClassHandlerAttribute))]
            public void RegisterTestClassHandler(IClassHandler handler, TestClassHandlerAttribute handlerAttribute)
            {
                RegisterClassCalled = true;
            }

            [PropertyAttributeDiscoveredHandler(typeof(TestPropertyHandlerAttribute))]
            public void RegisterTestPropertyHandler(IPropertyHandler handler, TestPropertyHandlerAttribute handlerAttribute)
            {
                RegisterPropertyCalled = true;
            }
        }

        private class TestAttributeBeforeDiscoveredHandler
        {
            public bool RegisterMethodCalled { get; private set; }
            public bool RegisterClassCalled { get; private set; }

            public bool RegisterPropertyCalled => _called == 2;

            private int _called = 0;

            [MethodAttributeDiscoveredHandler(typeof(TestMethodHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public void RegisterTestHandler(IMethodHandler handler, TestMethodHandlerAttribute handlerAttribute)
            {
                RegisterMethodCalled = true;
            }

            [ClassAttributeDiscoveredHandler(typeof(TestClassHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public void RegisterTestClassHandler(IClassHandler handler, TestClassHandlerAttribute handlerAttribute)
            {
                RegisterClassCalled = true;
            }

            [PropertyAttributeDiscoveredHandler(typeof(TestPropertyHandlerAttribute), HandleAlreadyDiscoveredAttributes = false)]
            public void RegisterTestPropertyHandler(IPropertyHandler handler, TestPropertyHandlerAttribute handlerAttribute)
            {
                _called++;
            }
        }

        [TestClassHandler]
        private class TestHandler
        {
            [TestMethodHandler]
            public void RegisterTestHandler(object token) {}

            [TestPropertyHandler]
            public string TestProperty { get; set; }

            [TestPropertyHandler] public int _test = 5;
        }

        [Fact]
        public void MethodAttributeDiscoverShouldBeFailIfAnIncorectHandlerIsRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();

            // Act  
            Exception ex = Assert.Throws<ArgumentException>(
                    () => attributeDiscover.RegisterInstanceContainer(new InvalidTestMethodAttributeDiscoveredHandler())
                );
            ;

            // Assert
            Assert.Equal("Method handler Void RegisterTestMethodHandler() has incorrect parameters. Right definition is Handler(IMethodHandler, MethodHandlerAttribute)", ex.Message);
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterAttributeDiscoveredInstanceHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var attributeDiscoverInstanceHandler = new TestAttributeDiscoveredHandler();
            // Act  
            attributeDiscover.RegisterInstanceContainer(attributeDiscoverInstanceHandler);

            // Assert
            Assert.Equal(1, attributeDiscover.GetMethodAttributeDiscoveredHandlers(typeof(TestMethodHandlerAttribute)).Count());
            Assert.Equal(1, attributeDiscover.GetClassAttributeDiscoveredHandlers(typeof(TestClassHandlerAttribute)).Count());
            Assert.Equal(1, attributeDiscover.GetPropertyAttributeDiscoveredHandlers(typeof(TestPropertyHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldRegisterInstanceHandler()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var instanceHandler = new TestHandler();

            // Act  
            attributeDiscover.RegisterInstanceContainer(instanceHandler);

            // Assert
            Assert.Equal(1, attributeDiscover.GetMethodHandlers(typeof(TestMethodHandlerAttribute)).Count());
            Assert.Equal(1, attributeDiscover.GetClassHandlers(typeof(TestClassHandlerAttribute)).Count());
            Assert.Equal(2, attributeDiscover.GetPropertyHandlers(typeof(TestPropertyHandlerAttribute)).Count());
        }

        [Fact]
        public void AttributeDiscoverShouldBeCalledOnNewInstanceTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var attributeDiscoverInstanceHandler = new TestAttributeDiscoveredHandler();
            var instanceHandler = new TestHandler();

            attributeDiscover.RegisterInstanceContainer(attributeDiscoverInstanceHandler);

            // Act  
            attributeDiscover.RegisterInstanceContainer(instanceHandler);

            // Assert
            Assert.True(attributeDiscoverInstanceHandler.RegisterMethodCalled);
            Assert.True(attributeDiscoverInstanceHandler.RegisterClassCalled);
            Assert.True(attributeDiscoverInstanceHandler.RegisterPropertyCalled);
        }

        [Fact]
        public void MethodAttributeDiscoverShouldBeCalledAfterTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var attributeDiscoverInstanceHandler = new TestAttributeDiscoveredHandler();
            var instanceHandler = new TestHandler();

            attributeDiscover.RegisterInstanceContainer(instanceHandler);

            // Act  
            attributeDiscover.RegisterInstanceContainer(attributeDiscoverInstanceHandler);

            // Assert
            Assert.True(attributeDiscoverInstanceHandler.RegisterMethodCalled);
            Assert.True(attributeDiscoverInstanceHandler.RegisterClassCalled);
            Assert.True(attributeDiscoverInstanceHandler.RegisterPropertyCalled);
        }

        [Fact]
        public void MethodeAttributeDiscoverShouldNotBeCalledAfterTypeRegistered()
        {
            // Arrange
            var attributeDiscover = new AttributeDiscover();
            var attributeDiscoverInstanceHandler = new TestAttributeBeforeDiscoveredHandler();
            var instanceHandler = new TestHandler();

            attributeDiscover.RegisterInstanceContainer(instanceHandler);

            // Act  
            attributeDiscover.RegisterInstanceContainer(attributeDiscoverInstanceHandler);

            // Assert
            Assert.False(attributeDiscoverInstanceHandler.RegisterMethodCalled);
            Assert.False(attributeDiscoverInstanceHandler.RegisterClassCalled);
            Assert.False(attributeDiscoverInstanceHandler.RegisterPropertyCalled);
        }
    }
}
