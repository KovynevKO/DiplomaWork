using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Role { get; set; }
        public List<UserProduct> UserProducts { get; set; }
        public List<UserDialogue> UserDialogues { get; set; }
        public User()
        {
            UserProducts = new List<UserProduct>();
            UserDialogues = new List<UserDialogue>();
        }
    }
}
