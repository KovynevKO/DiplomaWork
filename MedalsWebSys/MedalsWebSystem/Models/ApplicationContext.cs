using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Dialogue> Dialogues { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDialogue>()
                .HasKey(t => new { t.UserId, t.DialogueId});

            modelBuilder.Entity<UserDialogue>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserDialogues)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserDialogue>()
                .HasOne(sc => sc.Dialogue)
                .WithMany(c => c.UserDialogues)
                .HasForeignKey(sc => sc.DialogueId);

            modelBuilder.Entity<UserProduct>()
                .HasKey(t => new { t.UserId, t.ProductId });

            modelBuilder.Entity<UserProduct>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserProducts)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserProduct>()
                .HasOne(sc => sc.Product)
                .WithMany(c => c.UserProducts)
                .HasForeignKey(sc => sc.ProductId);
        }
    }
}
