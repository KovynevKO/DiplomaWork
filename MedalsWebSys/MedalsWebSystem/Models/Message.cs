using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public User Sender { get; set; }
        public int DialogueId { get; set; }
        public Dialogue Dialogue { get; set; }
    }
}
