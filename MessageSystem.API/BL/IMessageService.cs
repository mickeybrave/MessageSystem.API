using MessageSystem.API.DAL;

namespace MessageSystem.API.BL
{
    public interface IMessageService
    {
        Task<Message> GetMessageForCountryAndDateAsync(string countryCode, DateTime date);
        Task<List<Message>> GetAllMessagesAsync();
        Task AddMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(int id);
    }
}
