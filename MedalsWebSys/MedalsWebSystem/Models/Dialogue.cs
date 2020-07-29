using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.Models
{
    public class Dialogue
    {
        public int DialogueId { get; set; }
        public List<Message> Messages { get; set; }
        public List<UserDialogue> UserDialogues { get; set; }

        public Dialogue()
        {
            UserDialogues = new List<UserDialogue>();
        }
    }
}
