using MessageSystem.API.BL;
using MessageSystem.API.Controllers;
using MessageSystem.API.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Moq;

namespace MessagesSystem.Tests
{
    public class MessagesControllerTests
    {
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly MessagesController _controller;

        public MessagesControllerTests()
        {
            _mockMessageService = new Mock<IMessageService>();
            _controller = new MessagesController(_mockMessageService.Object);
        }

        [Fact]
        public async Task GetMessageForCountryAndDate_Returns_Ok_Result()
        {
            // Arrange
            var countryCode = "US";
            var date = new DateTime(2024, 12, 25);
            var expectedMessage = new Message { Id = 1, CountryCode = "US", Greeting = "Happy Holidays!", StartDate = new DateTime(2024, 12, 1) };

            _mockMessageService.Setup(service => service.GetMessageForCountryAndDateAsync(countryCode, date))
                               .ReturnsAsync(expectedMessage);

            // Act
            var result = await _controller.GetMessageForCountryAndDate(countryCode, date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualMessage = Assert.IsAssignableFrom<Message>(okResult.Value);
            Assert.Equal(expectedMessage.Id, actualMessage.Id);
            Assert.Equal(expectedMessage.CountryCode, actualMessage.CountryCode);
            Assert.Equal(expectedMessage.Greeting, actualMessage.Greeting);
            Assert.Equal(expectedMessage.StartDate, actualMessage.StartDate);
        }

        [Fact]
        public async Task GetMessageForCountryAndDate_Returns_NotFound_Result()
        {
            // Arrange
            var countryCode = "US";
            var date = new DateTime(2024, 12, 25);

            _mockMessageService.Setup(service => service.GetMessageForCountryAndDateAsync(countryCode, date))
                               .ReturnsAsync((Message)null);

            // Act
            var result = await _controller.GetMessageForCountryAndDate(countryCode, date);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllMessages_Returns_Ok_Result()
        {
            // Arrange
            var expectedMessages = new List<Message>
            {
                new Message { Id = 1, CountryCode = "US", Greeting = "Happy Holidays!", StartDate = new DateTime(2024, 12, 1) },
                new Message { Id = 2, CountryCode = "CA", Greeting = "Season's Greetings!", StartDate = new DateTime(2024, 11, 25) }
            };

            _mockMessageService.Setup(service => service.GetAllMessagesAsync())
                               .ReturnsAsync(expectedMessages);

            // Act
            var result = await _controller.GetAllMessages();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualMessages = Assert.IsAssignableFrom<List<Message>>(okResult.Value);
            Assert.Equal(expectedMessages.Count, actualMessages.Count);
            for (int i = 0; i < expectedMessages.Count; i++)
            {
                Assert.Equal(expectedMessages[i].Id, actualMessages[i].Id);
                Assert.Equal(expectedMessages[i].CountryCode, actualMessages[i].CountryCode);
                Assert.Equal(expectedMessages[i].Greeting, actualMessages[i].Greeting);
                Assert.Equal(expectedMessages[i].StartDate, actualMessages[i].StartDate);
            }
        }

        [Fact]
        public async Task AddMessage_Returns_CreatedAtAction_Result()
        {
            // Arrange
            var newMessage = new Message { Id = 1, CountryCode = "US", Greeting = "Happy New Year!", StartDate = new DateTime(2025, 1, 1) };

            _mockMessageService.Setup(service => service.AddMessageAsync(It.IsAny<Message>()))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddMessage(newMessage);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Message>(createdAtActionResult.Value);
            Assert.Equal(newMessage.Id, returnValue.Id);
            Assert.Equal(newMessage.CountryCode, returnValue.CountryCode);
            Assert.Equal(newMessage.Greeting, returnValue.Greeting);
            Assert.Equal(newMessage.StartDate, returnValue.StartDate);
        }

        [Fact]
        public async Task UpdateMessage_Returns_NoContent_Result()
        {
            // Arrange
            var messageId = 1;
            var updatedMessage = new Message { Id = messageId, CountryCode = "US", Greeting = "Updated Greeting", StartDate = new DateTime(2024, 12, 1) };

            _mockMessageService.Setup(service => service.UpdateMessageAsync(It.IsAny<Message>()))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateMessage(messageId, updatedMessage);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteMessage_Returns_NoContent_Result()
        {
            // Arrange
            var messageId = 1;

            _mockMessageService.Setup(service => service.DeleteMessageAsync(messageId))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteMessage(messageId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
