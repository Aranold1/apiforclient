using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiForGptBlazor.Models;

public partial class ChatGptCloneTstDbContext : DbContext
{
    public ChatGptCloneTstDbContext()
    {
    }

    public ChatGptCloneTstDbContext(DbContextOptions<ChatGptCloneTstDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_pkey");

            entity.ToTable("chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fkuserid).HasColumnName("fkuserid");
            entity.Property(e => e.Theme)
                .HasColumnName("theme");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_pkey");
            entity.ToTable("message");
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnName("id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.Fkchatid).HasColumnName("fkchatid");
            entity.Property(e => e.Isgptauthor).HasColumnName("isgptauthor");

            entity.HasOne(d => d.Chat)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.Fkchatid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_chat_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
