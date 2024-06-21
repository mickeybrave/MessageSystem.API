using System.Linq.Expressions;

namespace MessageSystem.API.DAL
{
    public interface IMessageRepository
    {
        Task<Message> GetMessageForCountryAndDateAsync(string countryCode, DateTime date);
        Task<List<Message>> GetAllMessagesAsync();
        Task AddMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(int id);
        Task<Message> GetMessageById(int id);
        Task<Message> FirstOrDefaultAsync(Expression<Func<Message, bool>> predicate);

    }
}
