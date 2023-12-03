﻿// <auto-generated />
using System;
using Dumbogram.Database;
using Dumbogram.Models.Files;
using Dumbogram.Models.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dumbogram.Database.Migrations.Application
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231203162103_Added meta information to FilesGroup and added metadata for each file type")]
    partial class AddedmetainformationtoFilesGroupandaddedmetadataforeachfiletype
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "files_group_type", new[] { "avatars", "attached_photos", "attached_videos", "attached_documents" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "system_message_type", new[] { "user_joined", "user_left", "chat_created", "chat_description_edited", "chat_title_edited" });
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

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_date");

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

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

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

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_date");

                    b.Property<int>("MembershipRight")
                        .HasColumnType("integer")
                        .HasColumnName("membership_right");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

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

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_date");

                    b.Property<int>("MembershipStatus")
                        .HasColumnType("integer")
                        .HasColumnName("membership_status");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("ChatId", "MemberId")
                        .HasName("pk_chat_memberships");

                    b.HasIndex("MemberId")
                        .HasDatabaseName("ix_chat_memberships_member_id");

                    b.ToTable("chat_memberships", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Models.Files.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_date");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint")
                        .HasColumnName("file_size");

                    b.Property<Guid?>("FilesGroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("files_group_id");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mime_type");

                    b.Property<string>("OriginalFileName")
                        .HasColumnType("text")
                        .HasColumnName("original_filename");

                    b.Property<string>("StoredFileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("stored_filename");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.Property<string>("file_type")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)")
                        .HasColumnName("file_type");

                    b.HasKey("Id")
                        .HasName("pk_file");

                    b.HasIndex("FilesGroupId")
                        .HasDatabaseName("ix_file_files_group_id");

                    b.ToTable("file", (string)null);

                    b.HasDiscriminator<string>("file_type").HasValue("File");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FilesGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_date");

                    b.Property<FilesGroupType>("GroupType")
                        .HasColumnType("files_group_type")
                        .HasColumnName("group_type");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_files_groups");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_files_groups_owner_id");

                    b.ToTable("files_groups", (string)null);
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.Message", b =>
                {
                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_date");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uuid")
                        .HasColumnName("sender_id");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.Property<string>("message_type")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)")
                        .HasColumnName("message_type");

                    b.HasKey("ChatId", "Id")
                        .HasName("pk_messages");

                    b.HasIndex("ChatId")
                        .HasDatabaseName("ix_messages_chat_id");

                    b.HasIndex("CreatedDate")
                        .HasDatabaseName("ix_messages_created_date");

                    b.HasIndex("SenderId")
                        .HasDatabaseName("ix_messages_sender_id");

                    b.ToTable("messages", (string)null);

                    b.HasDiscriminator<string>("message_type").HasValue("Message");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Dumbogram.Models.Users.UserProfile", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_date");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

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

            modelBuilder.Entity("Dumbogram.Models.Files.FileAnimation", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Files.File");

                    b.HasDiscriminator().HasValue("animation");
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FileDocument", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Files.File");

                    b.HasDiscriminator().HasValue("document");
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FilePhoto", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Files.File");

                    b.HasDiscriminator().HasValue("photo");
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FileVideo", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Files.File");

                    b.HasDiscriminator().HasValue("video");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.SystemMessage", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Messages.Message");

                    b.Property<SystemMessageType>("SystemMessageType")
                        .HasColumnType("system_message_type")
                        .HasColumnName("system_message_type");

                    b.HasDiscriminator().HasValue("system_message");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessage", b =>
                {
                    b.HasBaseType("Dumbogram.Models.Messages.Message");

                    b.Property<int?>("RepliedMessageId")
                        .HasColumnType("integer")
                        .HasColumnName("replied_message_id");

                    b.HasIndex("ChatId", "RepliedMessageId")
                        .HasDatabaseName("ix_messages_chat_id_replied_message_id");

                    b.HasDiscriminator().HasValue("user_message");
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

            modelBuilder.Entity("Dumbogram.Models.Files.File", b =>
                {
                    b.HasOne("Dumbogram.Models.Files.FilesGroup", "FilesGroup")
                        .WithMany("Files")
                        .HasForeignKey("FilesGroupId")
                        .HasConstraintName("fk_file_files_groups_files_group_id");

                    b.Navigation("FilesGroup");
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FilesGroup", b =>
                {
                    b.HasOne("Dumbogram.Models.Users.UserProfile", "Owner")
                        .WithMany("FilesGroups")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_files_groups_user_profiles_owner_id");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.Message", b =>
                {
                    b.HasOne("Dumbogram.Models.Chats.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_chats_chat_id");

                    b.HasOne("Dumbogram.Models.Users.UserProfile", "SenderProfile")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId")
                        .HasConstraintName("fk_messages_user_profiles_sender_id");

                    b.Navigation("Chat");

                    b.Navigation("SenderProfile");
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FileAnimation", b =>
                {
                    b.OwnsOne("Dumbogram.Models.Files.FileAnimationMetadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("FileAnimationId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Duration")
                                .HasColumnType("integer");

                            b1.Property<int>("Height")
                                .HasColumnType("integer");

                            b1.Property<int>("Width")
                                .HasColumnType("integer");

                            b1.HasKey("FileAnimationId")
                                .HasName("pk_file");

                            b1.ToTable("file");

                            b1.ToJson("animation_metadata");

                            b1.WithOwner()
                                .HasForeignKey("FileAnimationId")
                                .HasConstraintName("fk_file_file_file_animation_id");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FileDocument", b =>
                {
                    b.OwnsOne("Dumbogram.Models.Files.FileDocumentMetadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("FileDocumentId")
                                .HasColumnType("uuid");

                            b1.Property<bool>("Executable")
                                .HasColumnType("boolean");

                            b1.HasKey("FileDocumentId")
                                .HasName("pk_file");

                            b1.ToTable("file");

                            b1.ToJson("document_metadata");

                            b1.WithOwner()
                                .HasForeignKey("FileDocumentId")
                                .HasConstraintName("fk_file_file_file_document_id");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FilePhoto", b =>
                {
                    b.OwnsOne("Dumbogram.Models.Files.FilePhotoMetadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("FilePhotoId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Height")
                                .HasColumnType("integer");

                            b1.Property<int>("Width")
                                .HasColumnType("integer");

                            b1.HasKey("FilePhotoId")
                                .HasName("pk_file");

                            b1.ToTable("file");

                            b1.ToJson("photo_metadata");

                            b1.WithOwner()
                                .HasForeignKey("FilePhotoId")
                                .HasConstraintName("fk_file_file_file_photo_id");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FileVideo", b =>
                {
                    b.OwnsOne("Dumbogram.Models.Files.FileVideoMetadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("FileVideoId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Duration")
                                .HasColumnType("integer");

                            b1.Property<int>("Height")
                                .HasColumnType("integer");

                            b1.Property<int>("Width")
                                .HasColumnType("integer");

                            b1.HasKey("FileVideoId")
                                .HasName("pk_file");

                            b1.ToTable("file");

                            b1.ToJson("video_metadata");

                            b1.WithOwner()
                                .HasForeignKey("FileVideoId")
                                .HasConstraintName("fk_file_file_file_video_id");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.SystemMessage", b =>
                {
                    b.OwnsOne("Dumbogram.Models.Messages.SystemMessageDetails", "SystemMessageDetails", b1 =>
                        {
                            b1.Property<Guid>("SystemMessageChatId")
                                .HasColumnType("uuid");

                            b1.Property<int>("SystemMessageId")
                                .HasColumnType("integer");

                            b1.Property<string>("NewDescription")
                                .HasColumnType("text");

                            b1.Property<string>("NewTitle")
                                .HasColumnType("text");

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

            modelBuilder.Entity("Dumbogram.Models.Messages.UserMessage", b =>
                {
                    b.HasOne("Dumbogram.Models.Messages.Message", "RepliedMessage")
                        .WithMany("Replies")
                        .HasForeignKey("ChatId", "RepliedMessageId")
                        .HasConstraintName("fk_messages_messages_chat_id_replied_message_id");

                    b.OwnsOne("Dumbogram.Models.Messages.UserMessageContent", "Content", b1 =>
                        {
                            b1.Property<Guid>("UserMessageChatId")
                                .HasColumnType("uuid");

                            b1.Property<int>("UserMessageId")
                                .HasColumnType("integer");

                            b1.Property<string>("Text")
                                .HasMaxLength(2048)
                                .HasColumnType("character varying(2048)");

                            b1.HasKey("UserMessageChatId", "UserMessageId")
                                .HasName("pk_messages");

                            b1.ToTable("messages");

                            b1.ToJson("Content");

                            b1.WithOwner()
                                .HasForeignKey("UserMessageChatId", "UserMessageId")
                                .HasConstraintName("fk_messages_messages_user_message_chat_id_user_message_id");
                        });

                    b.Navigation("Content")
                        .IsRequired();

                    b.Navigation("RepliedMessage");
                });

            modelBuilder.Entity("Dumbogram.Models.Chats.Chat", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Messages");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Dumbogram.Models.Files.FilesGroup", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("Dumbogram.Models.Messages.Message", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Dumbogram.Models.Users.UserProfile", b =>
                {
                    b.Navigation("FilesGroups");

                    b.Navigation("Memberships");

                    b.Navigation("Messages");

                    b.Navigation("OwnedChats");

                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
