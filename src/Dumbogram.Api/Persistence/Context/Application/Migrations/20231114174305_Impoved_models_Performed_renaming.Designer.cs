﻿// <auto-generated />

#nullable disable

using Dumbogram.Api.Persistence.Context.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231114174305_Impoved_models_Performed_renaming")]
    partial class Impoved_models_Performed_renaming
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_chats");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_chats_owner_id");

                    b.ToTable("chats", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.ChatMemberPermission", b =>
                {
                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid")
                        .HasColumnName("member_id");

                    b.Property<int>("MembershipRight")
                        .HasColumnType("integer")
                        .HasColumnName("membership_right");

                    b.HasKey("ChatId", "MemberId")
                        .HasName("pk_chat_member_permissions");

                    b.HasIndex("MemberId")
                        .HasDatabaseName("ix_chat_member_permissions_member_id");

                    b.ToTable("chat_member_permissions", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.ChatMembership", b =>
                {
                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid")
                        .HasColumnName("member_id");

                    b.Property<int>("MembershipStatus")
                        .HasColumnType("integer")
                        .HasColumnName("membership_status");

                    b.HasKey("ChatId", "MemberId")
                        .HasName("pk_chat_memberships");

                    b.HasIndex("MemberId")
                        .HasDatabaseName("ix_chat_memberships_member_id");

                    b.ToTable("chat_memberships", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.ChatMessage", b =>
                {
                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("Message")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("message");

                    b.Property<int>("MessageType")
                        .HasColumnType("integer")
                        .HasColumnName("message_type");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid")
                        .HasColumnName("sender_id");

                    b.HasKey("ChatId", "Id")
                        .HasName("pk_chat_messages");

                    b.HasIndex("ChatId")
                        .HasDatabaseName("ix_chat_messages_chat_id");

                    b.HasIndex("CreatedDate")
                        .HasDatabaseName("ix_chat_messages_created_date");

                    b.HasIndex("SenderId")
                        .HasDatabaseName("ix_chat_messages_sender_id");

                    b.ToTable("chat_messages", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Core.Users.Models.UserProfile", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("username");

                    b.HasKey("UserId")
                        .HasName("pk_user_profiles");

                    b.HasAlternateKey("Username")
                        .HasName("ak_user_profiles_username");

                    b.ToTable("user_profiles", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.Chat", b =>
                {
                    b.HasOne("Dumbogram.Core.Users.Models.UserProfile", "OwnerProfile")
                        .WithMany("OwnedChats")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chats_user_profiles_owner_id");

                    b.Navigation("OwnerProfile");
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.ChatMemberPermission", b =>
                {
                    b.HasOne("Dumbogram.Core.Chats.Models.Chat", "Chat")
                        .WithMany("Permissions")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_member_permissions_chats_chat_id");

                    b.HasOne("Dumbogram.Core.Users.Models.UserProfile", "MemberProfile")
                        .WithMany("Permissions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_member_permissions_user_profiles_member_id");

                    b.Navigation("Chat");

                    b.Navigation("MemberProfile");
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.ChatMembership", b =>
                {
                    b.HasOne("Dumbogram.Core.Chats.Models.Chat", "Chat")
                        .WithMany("Memberships")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_memberships_chats_chat_id");

                    b.HasOne("Dumbogram.Core.Users.Models.UserProfile", "MemberProfile")
                        .WithMany("Memberships")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_memberships_user_profiles_member_id");

                    b.Navigation("Chat");

                    b.Navigation("MemberProfile");
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.ChatMessage", b =>
                {
                    b.HasOne("Dumbogram.Core.Chats.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_messages_chats_chat_id");

                    b.HasOne("Dumbogram.Core.Users.Models.UserProfile", "SenderProfile")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_messages_user_profiles_sender_id");

                    b.Navigation("Chat");

                    b.Navigation("SenderProfile");
                });

            modelBuilder.Entity("Dumbogram.Core.Chats.Models.Chat", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Messages");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Dumbogram.Core.Users.Models.UserProfile", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Messages");

                    b.Navigation("OwnedChats");

                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
