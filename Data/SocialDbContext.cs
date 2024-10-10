using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data.Models;
using System.Reflection.Emit;

namespace SimpleSocialApp.Data
{
    public class SocialDbContext : IdentityDbContext<AppUser>
    {

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<Media> Media { get; set; }




        public SocialDbContext(DbContextOptions<SocialDbContext> options)
            : base(options)
        {
            //this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            //User
            
            builder.Entity<AppUser>().
                HasMany(u => u.Posts).
                WithOne(p => p.User).
                HasForeignKey(p => p.UserId).
                OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<AppUser>()
                .HasMany(u => u.Conversations)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserConversation"));

            builder.Entity<AppUser>().
                HasMany(u => u.Friendships).
                WithOne(x => x.User).
                HasForeignKey(f => f.User1Id).
                OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasOne(u => u.Media)
                .WithOne(m => m.User)
                .HasForeignKey<Media>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Friendship

            builder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.User2Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Friendship>()
                .HasIndex(f => new { f.User1Id, f.User2Id })
                .IsUnique();

           //Post
            builder.Entity<Post>()
                .HasMany(p => p.Reacts)
                .WithOne(r => r.Post).
                HasForeignKey(r => r.PostId).
                OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Post>().
                HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Post>().
                HasMany(p => p.Media).
                WithOne(m => m.Post).
                HasForeignKey(m => m.PostId).
                OnDelete(DeleteBehavior.Cascade); 


            //Conversation
            builder.Entity<Conversation>().
                HasMany(c => c.Messages).
                WithOne(m => m.Conversation).
                HasForeignKey(m => m.ConversationId).
                OnDelete(DeleteBehavior.Cascade);

            //Comment
            builder.Entity<Comment>()
                .HasMany(c => c.Reacts)
                .WithOne(r => r.Comment).
                HasForeignKey(r => r.CommentId).
                OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);
            //Reaction
            builder.Entity<Reaction>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId).
                OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);      
        }

    }
}
