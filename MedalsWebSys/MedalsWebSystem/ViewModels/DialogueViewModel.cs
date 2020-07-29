using MedalsWebSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.ViewModels
{
    public class DialogueViewModel
    {
        public Dialogue Dialogue { get; set; }
        public User User2 { get; set; }
        public List<Message> Messages { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public NewMessageViewModel NewMessageViewModel { get; set; }
    }
}
