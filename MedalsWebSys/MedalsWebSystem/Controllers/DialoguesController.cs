using MedalsWebSystem.Models;
using MedalsWebSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedalsWebSystem.Controllers
{
    public class DialoguesController : Controller
    {
        private ApplicationContext db;
        public DialoguesController(ApplicationContext context)
        {
            db = context;
        }

        [Authorize]
        [HttpGet]
        public ViewResult DialogueWith(int userId, int page=1)
        {
            int pageSize = 100;
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;

            User user1 = db.Users.Include(xc => xc.UserDialogues).ThenInclude(xc => xc.Dialogue).FirstOrDefault(x => x.Email == User.Identity.Name);
            User user2 = db.Users.Include(xc => xc.UserDialogues).ThenInclude(xc => xc.Dialogue).FirstOrDefault(x => x.UserId == userId);
            int dialogueId = FindDialogueId(user1, user2);

            Dialogue dialogue = db.Dialogues.Include(xc => xc.Messages).ThenInclude(x => x.Sender).FirstOrDefault(x => x.DialogueId == dialogueId);
            Dialogue dialogue2 = db.Dialogues.Include(xc => xc.UserDialogues).FirstOrDefault(x => x.DialogueId == dialogueId);
            List<Message> messages = dialogue.Messages.OrderByDescending(x => x.Time).ToList();
            UserDialogue ud = dialogue2.UserDialogues.FirstOrDefault(x => x.UserId == user1.UserId);
            ud.IsRead = true;
            db.Update(ud);
            db.SaveChanges();

            var count = messages.Count();
            ViewBag.SearchCount = count;
            var items = messages.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            DialogueViewModel d1 = new DialogueViewModel
            {
                Dialogue = dialogue,
                User2 = user2,
                Messages = messages,
                PageViewModel = pageViewModel
            };

            return View(d1);
        }

        public IActionResult DialogueWith(string Text, int user2Id, int dialogueId)
        {
            if(Text.Length>250)
            {
                Text = Text.Substring(250);
            }
            User user1 = db.Users.Include(xc => xc.UserDialogues).ThenInclude(xc => xc.Dialogue).FirstOrDefault(x => x.Email == User.Identity.Name);
            Message message = new Message {Text=Text, Time= DateTime.UtcNow , Sender= user1 };
            Dialogue dialogue = db.Dialogues.Include(xc=>xc.UserDialogues).FirstOrDefault(x => x.DialogueId == dialogueId);
            UserDialogue ud = dialogue.UserDialogues.FirstOrDefault(x => x.UserId == user2Id);
            ud.IsRead = false;
            db.Update(ud);
            dialogue.Messages = new List<Message>();
            dialogue.Messages.Add(message);
            db.Update(dialogue);
            db.SaveChanges();
            return RedirectToAction("DialogueWith", new { userId = user2Id });
        }

        private int FindDialogueId(User user1, User user2)
        {
            List<Dialogue> dialogues1 = user1.UserDialogues.Select(x => x.Dialogue).ToList();
            List<Dialogue> dialogues2 = user2.UserDialogues.Select(x => x.Dialogue).ToList();

            foreach(Dialogue d1 in dialogues1)
            {
                foreach(Dialogue d2 in dialogues2)
                {
                    if (d1.DialogueId == d2.DialogueId)
                        return d1.DialogueId;
                }
            }

            Dialogue newD = new Dialogue();
            db.Dialogues.Add(newD);
            db.SaveChanges();

            user1.UserDialogues.Add(new UserDialogue { UserId = user1.UserId, DialogueId = newD.DialogueId });
            user2.UserDialogues.Add(new UserDialogue { UserId = user2.UserId, DialogueId = newD.DialogueId });
            db.SaveChanges();
            return newD.DialogueId;
        }

        [Authorize]
        public ViewResult MyDialogues()
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;

            User user1 = db.Users.Include(xc=>xc.UserDialogues).ThenInclude(xc=>xc.Dialogue).FirstOrDefault(x => x.Email == User.Identity.Name);

            List <Dialogue> mainDialogues = new List<Dialogue>();
            List<UserAndRead> usersAndRead = new List <UserAndRead>();
            var dialogues = user1.UserDialogues.Select(xc => xc.Dialogue).ToList();
            foreach(var d in dialogues)
            {
                mainDialogues.Add(db.Dialogues.Include(xc=>xc.UserDialogues).ThenInclude(xc=>xc.User).FirstOrDefault(x => x.DialogueId == d.DialogueId));
            }
            foreach(Dialogue d in mainDialogues)
            {
                UserDialogue ud = d.UserDialogues.FirstOrDefault(x => x.UserId == user1.UserId);
                bool IsRead = ud.IsRead;

                ud = d.UserDialogues.FirstOrDefault(x => x.UserId != user1.UserId);
                User user = ud.User;
                usersAndRead.Add(new UserAndRead { User = user, IsRead = IsRead });
            }
            usersAndRead = usersAndRead.OrderBy(x => x.IsRead).ToList();
            return View(usersAndRead);
        }
    }
}
