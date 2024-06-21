using MessageSystem.API.DAL;

namespace MessageSystem.API.BL
{

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;
        private readonly ILogger<IMessageService> _logger;

        public MessageService(IMessageRepository repository, ILogger<IMessageService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Message> GetMessageForCountryAndDateAsync(string countryCode, DateTime date)
        {
            try
            {
                _logger.LogInformation("Getting message for country {CountryCode} and date {Date}", countryCode, date);

                var messages = await _repository.GetAllMessagesAsync();

                var countryMessages = messages.Where(m => m.CountryCode == countryCode).ToList();
                var universalMessages = messages.Where(m => m.CountryCode == "AAA").ToList();

                // First, try to find a valid type B (time-limited) message for the country
                var validTypeBMessage = countryMessages
                    .Where(m => m.EndDate.HasValue && m.StartDate <= date && m.EndDate >= date)
                    .OrderByDescending(m => m.StartDate)
                    .FirstOrDefault();

                if (validTypeBMessage != null)
                {
                    _logger.LogInformation("Found valid type B message with ID {MessageId}", validTypeBMessage.Id);
                    return validTypeBMessage;
                }

                // If no valid type B message, look for a type A (permanent) message for the country
                var validTypeAMessage = countryMessages
                    .Where(m => !m.EndDate.HasValue && m.StartDate <= date)
                    .OrderByDescending(m => m.StartDate)
                    .FirstOrDefault();

                if (validTypeAMessage != null)
                {
                    _logger.LogInformation("Found valid type A message with ID {MessageId}", validTypeAMessage.Id);
                    return validTypeAMessage;
                }

                // If no country-specific message found, look for a universal message
                var validUniversalMessage = universalMessages
                    .Where(m => m.StartDate <= date && (!m.EndDate.HasValue || m.EndDate >= date))
                    .OrderByDescending(m => m.StartDate)
                    .FirstOrDefault();

                if (validUniversalMessage != null)
                {
                    _logger.LogInformation("Found valid universal message with ID {MessageId}", validUniversalMessage.Id);
                    return validUniversalMessage;
                }

                _logger.LogInformation("No valid message found for country {CountryCode} and date {Date}", countryCode, date);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting message for country {CountryCode} and date {Date}", countryCode, date);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public Task<List<Message>> GetAllMessagesAsync()
        {
            return _repository.GetAllMessagesAsync();
        }

        public Task<Message> GetMessageById(int id)
        {
            return _repository.GetMessageById(id);
        }

        public async Task AddMessageAsync(Message message)
        {
            message.StartDate = message.StartDate.ToUniversalTime();
            message.EndDate = message.EndDate?.ToUniversalTime();

            await ValidateTypeAMessageAsync(message);
            await ValidateTypeBMessageAsync(message);

            await _repository.AddMessageAsync(message);
        }

        private async Task ValidateTypeAMessageAsync(Message message)
        {
            if (message.EndDate == null) // Type (A) message
            {
                var existingMessage = await _repository.FirstOrDefaultAsync(m =>
                    m.CountryCode == message.CountryCode &&
                    m.EndDate == null && // Type (A)
                    m.StartDate == message.StartDate);

                if (existingMessage != null)
                {
                    throw new Exception("Multiple (A)-type messages starting at the same date and time are not allowed.");
                }
            }
        }

        private async Task ValidateTypeBMessageAsync(Message message)
        {
            if (message.EndDate != null) // Type (B) message
            {
                var existingMessage = await _repository.FirstOrDefaultAsync(m =>
                    m.CountryCode == message.CountryCode &&
                    m.EndDate != null && // Type (B)
                    m.StartDate == message.StartDate &&
                    m.EndDate == message.EndDate);

                if (existingMessage != null)
                {
                    throw new Exception("Multiple (B)-type messages cannot be duplicated for a specific date and time in a specific country.");
                }
            }
        }

        public Task UpdateMessageAsync(Message message)
        {
            message.StartDate = message.StartDate.ToUniversalTime();
            message.EndDate = message.EndDate?.ToUniversalTime();
            return _repository.UpdateMessageAsync(message);
        }

        public Task DeleteMessageAsync(int id)
        {
            return _repository.DeleteMessageAsync(id);
        }
    }
}
