using MessageSystem.API.DAL;

namespace MessageSystem.API.BL
{

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;

        public MessageService(IMessageRepository repository)
        {
            _repository = repository;
        }
        public async Task<Message> GetMessageForCountryAndDateAsync(string countryCode, DateTime date)
        {
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
                return validTypeBMessage;
            }

            // If no valid type B message, look for a type A (permanent) message for the country
            var validTypeAMessage = countryMessages
                .Where(m => !m.EndDate.HasValue && m.StartDate <= date)
                .OrderByDescending(m => m.StartDate)
                .FirstOrDefault();

            if (validTypeAMessage != null)
            {
                return validTypeAMessage;
            }

            // If no country-specific message found, look for a universal message
            var validUniversalMessage = universalMessages
                .Where(m => m.StartDate <= date && (!m.EndDate.HasValue || m.EndDate >= date))
                .OrderByDescending(m => m.StartDate)
                .FirstOrDefault();

            return validUniversalMessage;
        }

        public Task<List<Message>> GetAllMessagesAsync()
        {
            return _repository.GetAllMessagesAsync();
        }

        public Task AddMessageAsync(Message message)
        {
            message.StartDate = message.StartDate.ToUniversalTime();
            message.EndDate = message.EndDate?.ToUniversalTime();
            return _repository.AddMessageAsync(message);
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
