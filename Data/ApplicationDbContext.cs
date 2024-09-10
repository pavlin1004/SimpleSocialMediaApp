using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data.Models;
using System.Reflection.Emit;

namespace SimpleSocialApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(u => u.Posts).WithOne(p => p.User).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>().HasMany(u => u.Conversations).WithMany(c => c.Friends).UsingEntity(j => j.ToTable("UserConversation"));

            builder.Entity<Friendship>().HasOne(f => f.User).WithMany(u => u.Friendships).HasForeignKey(f => f.User1Id).OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Friendship>().HasOne(f => f.Friend).WithMany().HasForeignKey(f => f.User2Id).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Friendship>().HasIndex(f => new { f.User1Id, f.User2Id }).IsUnique();

            builder.Entity<Post>().HasMany(p => p.Reacts).WithOne().HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Post>().HasMany(p => p.Comments).WithOne().HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Conversation>().HasMany(c => c.Messages).WithOne().HasForeignKey(m => m.ConversationId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>().HasMany(c => c.Comments).WithOne(c => c.ParentComment).HasForeignKey(p => p.ParentCommentId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>().HasOne(s => s.Sender).WithMany().HasForeignKey(s => s.SenderId).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
