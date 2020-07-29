using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.ViewModels
{
    public class NewMessageViewModel
    {
        [Required(ErrorMessage = "Сообщение пустое")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Сообщение должно быть не больше 250 символов")]
        public string Text;

        public int dialogueId;
        public int user2Id;
    }
}
