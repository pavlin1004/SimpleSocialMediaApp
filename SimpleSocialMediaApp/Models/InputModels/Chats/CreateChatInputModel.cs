using SimpleSocialApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Models.InputModels.Chat
{
    public class CreateChatInputModel
    {
        [Required(ErrorMessage = "Title required!")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "At least one user must be added to create a new chat!")]
        [MinLength(1, ErrorMessage = "You must select at least one participant.")]
        public required List<string> ParticipantsIds { get; set; }
    }
}
