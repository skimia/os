using Skimia.Extensions.Messages.Abstractions;
using System;
using System.Linq;
using Skimia.Extensions.Discovery.Attributes;
using Xunit;
using TestMessage = Skimia.Extensions.Messages.Tests.Sample.TestMessage;

namespace Skimia.Extensions.Messages.Tests
{
    public class MessageDispatcherTests
    {
        private static class InvalidTestMessageHandler
        {
            [MessageHandler(typeof(TestMessage))]
            public static void MessageHandler() { }
        }

        private static class ExceptionTestMessageHandler
        {
            [MessageHandler(typeof(TestMessage))]
            public static void MessageHandler()
            {
                throw new Exception();
            }
        }

        private static class TestMessageHandler
        {
            public static bool HandlerCalled { get; set; }

            public static int HandlersCalled = 0;

            [MessageHandler(typeof(TestMessage))]
            public static void MessageHandler(object token, TestMessage message)
            {
                HandlerCalled = true;
                HandlersCalled++;
                message.BlockProgression();
            }

            [MessageHandler(typeof(TestMessage))]
            public static void SecondMessageHandler(object token, TestMessage message)
            {
                HandlerCalled = true;
                HandlersCalled++;
                message.BlockProgression();
            }
        }

        [Fact]
        public void MessageDispatcherShouldFailOnIncorrectHandler()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();
            var attributeDiscover = new AttributeDiscover();

            attributeDiscover.RegisterInstanceContainer(messageDispatcher);

            // Act  
            Exception ex = Assert.Throws<ArgumentException>(
                    () => attributeDiscover.RegisterStaticContainer(typeof(InvalidTestMessageHandler))
                );

            // Assert
            Assert.Equal("Message handler Void MessageHandler() has incorrect parameters. Right definition is Handler(object, IMessage)", ex.Message);
        }

        [Fact]
        public void MessageHandlerShouldHaveCorrectValues()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();
            var attributeDiscover = new AttributeDiscover();

            attributeDiscover.RegisterInstanceContainer(messageDispatcher);
            attributeDiscover.RegisterStaticContainer(typeof(TestMessageHandler));

            // Act  
            var handler = messageDispatcher.GetHandlers(typeof(TestMessage)).First();

            // Assert
            Assert.Null(handler.Container);
            Assert.Equal(typeof(TestMessageHandler), handler.ContainerType);
            Assert.Equal(typeof(TestMessageHandler), handler.ContainerType);
            Assert.Equal(typeof(TestMessage), handler.MessageType);
            Assert.Equal(typeof(object), handler.TokenType);
        }

        [Fact]
        public void MessageDispatcherShouldDispatchWithTask()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();
            var dispatcherTask = new MessageDispatcherTask(messageDispatcher, this);
            var attributeDiscover = new AttributeDiscover();
            var message = new TestMessage();

            attributeDiscover.RegisterInstanceContainer(messageDispatcher);
            attributeDiscover.RegisterStaticContainer(typeof(TestMessageHandler));
            dispatcherTask.Start();

            // Act  
            messageDispatcher.Enqueue(message);
            message.Wait(TimeSpan.Zero);

            // Assert
            Assert.True(TestMessageHandler.HandlerCalled);
            Assert.True(message.IsDispatched);

            //If Message already dispatched
            Assert.False(message.Wait());

            dispatcherTask.Stop();
        }

        [Fact]
        public void MessageDispatcherShouldNotDispatchWhenIsStopped()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();
            var dispatcherTask = new MessageDispatcherTask(messageDispatcher);
            var attributeDiscover = new AttributeDiscover();
            var message = new TestMessage();

            attributeDiscover.RegisterInstanceContainer(messageDispatcher);
            attributeDiscover.RegisterStaticContainer(typeof(TestMessageHandler));

            messageDispatcher.Stop();
            dispatcherTask.Start();

            // Act  
            messageDispatcher.Enqueue(message);
            message.Wait(TimeSpan.FromMilliseconds(100));

            // Assert
            Assert.False(TestMessageHandler.HandlerCalled);
            Assert.False(message.IsDispatched);

            dispatcherTask.Stop();
        }

        [Fact]
        public void IfAMessagePropagationIsBlockedMessageHandlerShouldNotCallOtherHandlers()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();
            var dispatcherTask = new MessageDispatcherTask(messageDispatcher);
            var attributeDiscover = new AttributeDiscover();
            var message = new TestMessage();

            attributeDiscover.RegisterInstanceContainer(messageDispatcher);
            attributeDiscover.RegisterStaticContainer(typeof(TestMessageHandler));

            dispatcherTask.Start();

            // Act  
            messageDispatcher.Enqueue(message);
            message.Wait();

            // Assert
            Assert.Equal(1, TestMessageHandler.HandlersCalled);
            Assert.True(message.Canceled);

            dispatcherTask.Stop();
        }
    }
}
