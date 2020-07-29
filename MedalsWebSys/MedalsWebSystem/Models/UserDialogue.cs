using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.Models
{
    public class UserDialogue
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsRead { get; set; }
        public int DialogueId { get; set; }
        public Dialogue Dialogue { get; set; }
    }
}
