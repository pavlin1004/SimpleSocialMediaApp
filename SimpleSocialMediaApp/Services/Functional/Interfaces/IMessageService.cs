using SimpleSocialApp.Data.Models;

namespace SimpleSociaMedialApp.Services.Functional.Interfaces
{
    public interface IMessageService
    {
        public Task<Message?> GetMessageByIdAsync(string id);
        public Task<IEnumerable<Message>> GetConversationMessagesAsync(string conversationId);
        public Task CreateMessageAsync(Message m);
        public Task UpdateMessageAsync(Message m);
        public Task RemoveMessageAsync(string messageid);

    }
}
