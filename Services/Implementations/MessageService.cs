using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;
using SQLitePCL;
using System.CodeDom;

namespace SimpleSocialApp.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly SocialDbContext _context;

        public MessageService(SocialDbContext context)
        {
            _context = context;
        }

        public async Task<Message?> GetMessageByIdAsync(string messageId)
        {
            return await _context.Messages.Include(m => m.Media).FirstOrDefaultAsync(m => m.Id==messageId);
        }

        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(string conversationId)
        {
            return await _context.Messages.Include(m => m.Media).Where(m => m.ChatId==conversationId).OrderBy(m => m.CreatedDateTime).ToListAsync();
        }
        public async Task CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMessageAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMessageAsync(string messageId)
        {
            var message = await GetMessageByIdAsync(messageId);
            if(message==null)
            {
                throw new KeyNotFoundException("Message with that keys doesnt exist!");
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }      
    }
}
