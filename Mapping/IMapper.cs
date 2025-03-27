using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.Chats;

namespace SimpleSocialApp.Mapping
{
    public interface IMapper
    {
        public ChatViewModel MapToChatViewModel(Chat chat, string friendName,int count,int size);
        public ChatViewModel MapToChatViewModel(Chat chat, int count, int size);
    }
}
