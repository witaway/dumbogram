﻿// <auto-generated />
using System;
using Dumbogram.Database;
using Dumbogram.Models.Messages.SystemMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dumbogram.Migrations.Application
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "system_message_type", new[] { "user_joined", "user_left", "chat_description_edited", "chat_title_edited" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dumbogram.Models.Chats.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("ChatVisibility")
                        .HasColumnType("integer")
                        .HasColumnName("chat_visibility");

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

            modelBuilder.Entity("Dumbogram.Models.Chats.ChatMemberPermission", b =>
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

            modelBuilder.Entity("Dumbogram.Models.Chats.ChatMembership", b =>
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

            modelBuilder.Entity("Dumbogram.Models.Messages.Message", b =>
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

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)")
                        .HasColumnName("discriminator");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("subject_id");

                    b.HasKey("ChatId", "Id")
                        .HasName("pk_messages");

                    b.HasIndex("ChatId")
                        .HasDatabaseName("ix_messages_chat_id");

                    b.HasIndex("CreatedDate")
                        .HasDatabaseName("ix_messages_created_date");

                    b.HasIndex("SubjectId")
                        .HasDatabaseName("ix_messages_subject_id");

                    b.ToTable("messages", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Message");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessages.RegularUserMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("content");

                    b.Property<int?>("RegularUserMessageId")
                        .HasColumnType("integer")
                        .HasColumnName("regular_user_message_id");

                    b.Property<int?>("RepliedMessageId")
                        .HasColumnType("integer")
                        .HasColumnName("replied_message_id");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("subject_id");

                    b.Property<Guid>("SubjectProfileUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("subject_profile_user_id");

                    b.HasKey("Id")
                        .HasName("pk_regular_user_messages");

                    b.HasIndex("RegularUserMessageId")
                        .HasDatabaseName("ix_regular_user_messages_regular_user_message_id");

                    b.HasIndex("SubjectProfileUserId")
                        .HasDatabaseName("ix_regular_user_messages_subject_profile_user_id");

                    b.HasIndex("ChatId", "RepliedMessageId")
                        .HasDatabaseName("ix_regular_user_messages_chat_id_replied_message_id");

                    b.ToTable("regular_user_messages", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Models.Users.UserProfile", b =>
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

            modelBuilder.Entity("Dumbogram.Models.Messages.SystemMessages.SystemMessage", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Messages.Message");

                    b.Property<SystemMessageType>("SystemMessageType")
                        .HasColumnType("system_message_type")
                        .HasColumnName("system_message_type");

                    b.HasDiscriminator().HasValue("SystemMessage");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessages.UserMessage", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Messages.Message");

                    b.HasDiscriminator().HasValue("UserMessage");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessages.ForwardUserMessage", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Messages.UserMessages.UserMessage");

                    b.Property<Guid>("ForwardedChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("forwarded_chat_id");

                    b.Property<int>("ForwardedMessageId")
                        .HasColumnType("integer")
                        .HasColumnName("forwarded_message_id");

                    b.Property<int?>("RegularUserMessageId")
                        .HasColumnType("integer")
                        .HasColumnName("regular_user_message_id");

                    b.HasIndex("RegularUserMessageId")
                        .HasDatabaseName("ix_messages_regular_user_message_id");

                    b.HasIndex("ForwardedChatId", "ForwardedMessageId")
                        .HasDatabaseName("ix_messages_forwarded_chat_id_forwarded_message_id");

                    b.HasDiscriminator().HasValue("ForwardUserMessage");
                });

            modelBuilder.Entity("Dumbogram.Models.Chats.Chat", b =>
                {
                    b.HasOne("Dumbogram.Models.Users.UserProfile", "OwnerProfile")
                        .WithMany("OwnedChats")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chats_user_profiles_owner_id");

                    b.Navigation("OwnerProfile");
                });

            modelBuilder.Entity("Dumbogram.Models.Chats.ChatMemberPermission", b =>
                {
                    b.HasOne("Dumbogram.Models.Chats.Chat", "Chat")
                        .WithMany("Permissions")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_member_permissions_chats_chat_id");

                    b.HasOne("Dumbogram.Models.Users.UserProfile", "MemberProfile")
                        .WithMany("Permissions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_member_permissions_user_profiles_member_id");

                    b.Navigation("Chat");

                    b.Navigation("MemberProfile");
                });

            modelBuilder.Entity("Dumbogram.Models.Chats.ChatMembership", b =>
                {
                    b.HasOne("Dumbogram.Models.Chats.Chat", "Chat")
                        .WithMany("Memberships")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_memberships_chats_chat_id");

                    b.HasOne("Dumbogram.Models.Users.UserProfile", "MemberProfile")
                        .WithMany("Memberships")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_memberships_user_profiles_member_id");

                    b.Navigation("Chat");

                    b.Navigation("MemberProfile");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.Message", b =>
                {
                    b.HasOne("Dumbogram.Models.Chats.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_chats_chat_id");

                    b.HasOne("Dumbogram.Models.Users.UserProfile", "SubjectProfile")
                        .WithMany("Messages")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_user_profiles_subject_id");

                    b.Navigation("Chat");

                    b.Navigation("SubjectProfile");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessages.RegularUserMessage", b =>
                {
                    b.HasOne("Dumbogram.Models.Chats.Chat", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_regular_user_messages_chats_chat_id");

                    b.HasOne("Dumbogram.Models.Messages.UserMessages.RegularUserMessage", null)
                        .WithMany("Replies")
                        .HasForeignKey("RegularUserMessageId")
                        .HasConstraintName("fk_regular_user_messages_regular_user_messages_regular_user_me");

                    b.HasOne("Dumbogram.Models.Users.UserProfile", "SubjectProfile")
                        .WithMany()
                        .HasForeignKey("SubjectProfileUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_regular_user_messages_user_profiles_subject_profile_user_id");

                    b.HasOne("Dumbogram.Models.Messages.Message", "RepliedMessage")
                        .WithMany("Replies")
                        .HasForeignKey("ChatId", "RepliedMessageId")
                        .HasConstraintName("fk_regular_user_messages_messages_chat_id_replied_message_id");

                    b.Navigation("Chat");

                    b.Navigation("RepliedMessage");

                    b.Navigation("SubjectProfile");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.SystemMessages.SystemMessage", b =>
                {
                    b.OwnsOne("Dumbogram.Models.Messages.SystemMessages.SystemMessageDetails", "SystemMessageDetails", b1 =>
                        {
                            b1.Property<Guid>("SystemMessageChatId")
                                .HasColumnType("uuid");

                            b1.Property<int>("SystemMessageId")
                                .HasColumnType("integer");

                            b1.HasKey("SystemMessageChatId", "SystemMessageId")
                                .HasName("pk_messages");

                            b1.ToTable("messages");

                            b1.ToJson("SystemMessageDetails");

                            b1.WithOwner()
                                .HasForeignKey("SystemMessageChatId", "SystemMessageId")
                                .HasConstraintName("fk_messages_messages_system_message_chat_id_system_message_id");
                        });

                    b.Navigation("SystemMessageDetails");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessages.ForwardUserMessage", b =>
                {
                    b.HasOne("Dumbogram.Models.Messages.UserMessages.RegularUserMessage", null)
                        .WithMany("Forwards")
                        .HasForeignKey("RegularUserMessageId")
                        .HasConstraintName("fk_messages_regular_user_messages_regular_user_message_id");

                    b.HasOne("Dumbogram.Models.Messages.UserMessages.UserMessage", "ForwardedMessage")
                        .WithMany("Forwards")
                        .HasForeignKey("ForwardedChatId", "ForwardedMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_messages_forwarded_chat_id_forwarded_message_id");

                    b.Navigation("ForwardedMessage");
                });

            modelBuilder.Entity("Dumbogram.Models.Chats.Chat", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Messages");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.Message", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessages.RegularUserMessage", b =>
                {
                    b.Navigation("Forwards");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Dumbogram.Models.Users.UserProfile", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Messages");

                    b.Navigation("OwnedChats");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessages.UserMessage", b =>
                {
                    b.Navigation("Forwards");
                });
#pragma warning restore 612, 618
        }
    }
}
