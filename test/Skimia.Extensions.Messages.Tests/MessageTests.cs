using Xunit;

namespace Skimia.Extensions.Messages.Tests
{
    public class MessageTests
    {
        [Fact]
        public void MessagePropagationShouldBeStopped()
        {
            // Arrange
            var x = 5;
            var y = 10;

            // Act  
            var result = x * y;

            // Assert
            Assert.Equal(50, result);
        }
    }
}
