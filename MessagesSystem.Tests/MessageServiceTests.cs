using MessageSystem.API.BL;
using MessageSystem.API.DAL;
using Moq;

namespace MessagesSystem.Tests
{
    public class MessageServiceTests
    {
        private readonly Mock<IMessageRepository> _mockRepository;
        private readonly MessageService _messageService;

        public MessageServiceTests()
        {
            _mockRepository = new Mock<IMessageRepository>();
            _messageService = new MessageService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetMessageForCountryAndDate_Scenario1_TwoAMessages()
        {
            // Scenario 1: If two (A)-type messages are set for a country of departure,
            // the earlier message will expire just before the start date and time of the later message.

            // Arrange
            var countryCode = "US";
            var date = new DateTime(2024, 12, 25);

            var messageA1 = new Message { Id = 1, CountryCode = "US", Greeting = "Message A1", StartDate = new DateTime(2024, 12, 1) };
            var messageA2 = new Message { Id = 2, CountryCode = "US", Greeting = "Message A2", StartDate = new DateTime(2024, 12, 10) };

            var allMessages = new List<Message> { messageA1, messageA2 };

            _mockRepository.Setup(repo => repo.GetAllMessagesAsync()).ReturnsAsync(allMessages);

            // Act
            var resultMessage = await _messageService.GetMessageForCountryAndDateAsync(countryCode, date);

            // Assert
            Assert.NotNull(resultMessage);
            Assert.Equal(messageA2.Id, resultMessage.Id);
        }

        [Fact]
        public async Task GetMessageForCountryAndDate_Scenario2_AandBMessages_B_not_In_Range()
        {
            // Scenario 2: If both a permanent message of type (A) and a time-limited message of type (B) are available for the country of departure,
            // the message of type (B) will take precedence over the message of type (A).

            // Arrange
            var countryCode = "US";
            var date = new DateTime(2024, 12, 25);

            var messageA = new Message { Id = 1, CountryCode = "US", Greeting = "Message A", StartDate = new DateTime(2024, 12, 1) };
            var messageB = new Message { Id = 2, CountryCode = "US", Greeting = "Message B", StartDate = new DateTime(2024, 12, 10), EndDate = new DateTime(2024, 12, 20) };

            var allMessages = new List<Message> { messageA, messageB };

            _mockRepository.Setup(repo => repo.GetAllMessagesAsync()).ReturnsAsync(allMessages);

            // Act
            var resultMessage = await _messageService.GetMessageForCountryAndDateAsync(countryCode, date);

            // Assert
            Assert.NotNull(resultMessage);
            Assert.Equal(messageA.Id, resultMessage.Id);
        }

        [Fact]
        public async Task GetMessageForCountryAndDate_Scenario2_AandB_Messages_B_in_Range()
        {
            // Scenario 2: If both a permanent message of type (A) and a time-limited message of type (B) are available for the country of departure,
            // the message of type (B) will take precedence over the message of type (A).

            // Arrange
            var countryCode = "US";
            var date = new DateTime(2024, 12, 25);

            var messageA = new Message { Id = 1, CountryCode = "US", Greeting = "Message A", StartDate = new DateTime(2024, 12, 1) };
            var messageB = new Message { Id = 2, CountryCode = "US", Greeting = "Message B", StartDate = new DateTime(2024, 12, 10), EndDate = new DateTime(2024, 12, 30) };

            var allMessages = new List<Message> { messageA, messageB };

            _mockRepository.Setup(repo => repo.GetAllMessagesAsync()).ReturnsAsync(allMessages);

            // Act
            var resultMessage = await _messageService.GetMessageForCountryAndDateAsync(countryCode, date);

            // Assert
            Assert.NotNull(resultMessage);
            Assert.Equal(messageB.Id, resultMessage.Id);
        }

        [Fact]
        public async Task GetMessageForCountryAndDate_Scenario3_OnlyBMessage()
        {
            // Scenario 3: If only a time-limited message of type (B) is specified for the country of departure,
            // then the universal message for that date will be applied for any other date and time not specified.

            // Arrange
            var countryCode = "US";
            var date = new DateTime(2024, 12, 15);

            var messageB = new Message { Id = 2, CountryCode = "US", Greeting = "Message B", StartDate = new DateTime(2024, 12, 10), EndDate = new DateTime(2024, 12, 20) };
            var messageAAA = new Message { Id = 2, CountryCode = "AAA", Greeting = "Message Universal", StartDate = new DateTime(2024, 12, 10), EndDate = new DateTime(2024, 12, 20) };


            var allMessages = new List<Message> { messageB, messageAAA };

            _mockRepository.Setup(repo => repo.GetAllMessagesAsync()).ReturnsAsync(allMessages);

            // Act
            var resultMessage = await _messageService.GetMessageForCountryAndDateAsync(countryCode, date);

            // Assert
            Assert.NotNull(resultMessage);
            Assert.Equal(messageB.Id, resultMessage.Id);
        }

        [Fact]
        public async Task GetMessageForCountryAndDate_NoMessages_Returns_Null()
        {
            // Scenario: No messages available for the specified country and date.

            // Arrange
            var countryCode = "US";
            var date = new DateTime(2024, 12, 25);

            var allMessages = new List<Message>(); // Empty list

            _mockRepository.Setup(repo => repo.GetAllMessagesAsync()).ReturnsAsync(allMessages);

            // Act
            var resultMessage = await _messageService.GetMessageForCountryAndDateAsync(countryCode, date);

            // Assert
            Assert.Null(resultMessage);
        }
    }
}

