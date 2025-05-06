﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SimpleSocialApp.Data.Models;
using System.Reflection.Emit;

namespace SimpleSocialApp.Data
{
    public class SocialDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }

        public SocialDbContext(DbContextOptions<SocialDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureAppUser(builder);
            ConfigureFriendship(builder);
            ConfigureNotification(builder);
            ConfigurePost(builder);
            ConfigureChat(builder);
            ConfigureComment(builder);
            ConfigureReaction(builder);
            ConfigureMessage(builder);
        }

        private void ConfigureAppUser(ModelBuilder builder)
        {
            builder.Entity<AppUser>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasMany(u => u.Chats)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserChat"));

            builder.Entity<AppUser>()
                .HasOne(u => u.Media)
                .WithOne(m => m.User)
                .HasForeignKey<Media>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureFriendship(ModelBuilder builder)
        {
            builder.Entity<Friendship>()
                .HasOne(f => f.Sender)
                .WithMany(au => au.Friendships)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
                .HasOne(f => f.Receiver)
                .WithMany()
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
                .HasIndex(f => new { f.SenderId, f.ReceiverId })
                .IsUnique();
        }

        private void ConfigureNotification(ModelBuilder builder)
        {
            builder.Entity<Notification>()
                .HasOne(n => n.UserTo)
                .WithMany(u => u.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.UserFrom)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigurePost(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasMany(p => p.Reacts)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .HasMany(p => p.Media)
                .WithOne(m => m.Post)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureChat(ModelBuilder builder)
        {
            builder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureComment(ModelBuilder builder)
        {
            builder.Entity<Comment>()
                .HasMany(c => c.Reacts)
                .WithOne(r => r.Comment)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasMany(c => c.Media)
                .WithOne(m => m.Comment)
                .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // deleted when post gets deleted

            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void ConfigureReaction(ModelBuilder builder)
        {
            builder.Entity<Reaction>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reaction>()
                .HasIndex(r => new { r.UserId, r.PostId })
                .IsUnique();
        }

        private void ConfigureMessage(ModelBuilder builder)
        {
            builder.Entity<Message>()
                .HasMany(m => m.Media)
                .WithOne(m => m.Message)
                .HasForeignKey(m => m.MessageId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
