using System.Text.Json;

namespace MessageSystem.API.DAL
{

    public class MessageRepository : IMessageRepository
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages.json");
        private List<Message> _messages;

        public MessageRepository()
        {
            LoadMessages();
        }

        private void LoadMessages()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _messages = JsonSerializer.Deserialize<List<Message>>(json);
            }
            else
            {
                _messages = new List<Message>();
            }
        }

        private void SaveMessages()
        {
            var json = JsonSerializer.Serialize(_messages, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public async Task<Message> GetMessageForCountryAndDateAsync(string countryCode, DateTime date)
        {
            return await Task.Run(() =>
            {
                var message = _messages.FirstOrDefault(m =>
                    m.CountryCode == countryCode &&
                    m.StartDate <= date &&
                    (m.EndDate == null || m.EndDate >= date));

                return message;
            });
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await Task.FromResult(_messages);
        }

        public async Task AddMessageAsync(Message message)
        {
            await Task.Run(() =>
            {
                message.Id = _messages.Count > 0 ? _messages.Max(m => m.Id) + 1 : 1;
                _messages.Add(message);
                SaveMessages();
            });
        }

        public async Task UpdateMessageAsync(Message message)
        {
            await Task.Run(() =>
            {
                var existingMessage = _messages.FirstOrDefault(m => m.Id == message.Id);
                if (existingMessage != null)
                {
                    existingMessage.CountryCode = message.CountryCode;
                    existingMessage.Greeting = message.Greeting;
                    existingMessage.StartDate = message.StartDate;
                    existingMessage.EndDate = message.EndDate;
                    SaveMessages();
                }
            });
        }

        public async Task DeleteMessageAsync(int id)
        {
            await Task.Run(() =>
            {
                var message = _messages.FirstOrDefault(m => m.Id == id);
                if (message != null)
                {
                    _messages.Remove(message);
                    SaveMessages();
                }
            });
        }
    }
}
