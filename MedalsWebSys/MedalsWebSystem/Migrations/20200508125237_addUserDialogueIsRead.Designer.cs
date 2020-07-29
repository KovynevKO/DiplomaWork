﻿// <auto-generated />
using System;
using MedalsWebSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedalsWebSystem.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200508125237_addUserDialogueIsRead")]
    partial class addUserDialogueIsRead
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MedalsWebSystem.Models.Dialogue", b =>
                {
                    b.Property<int>("DialogueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("DialogueId");

                    b.ToTable("Dialogues");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DialogueId")
                        .HasColumnType("int");

                    b.Property<int?>("SenderUserId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId");

                    b.HasIndex("DialogueId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Diameter")
                        .HasColumnType("real");

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("UserAdderUserId")
                        .HasColumnType("int");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.Property<bool>("isMainCatalog")
                        .HasColumnType("bit");

                    b.Property<byte[]>("minPhoto")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ProductId");

                    b.HasIndex("UserAdderUserId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.UserDialogue", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("DialogueId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "DialogueId");

                    b.HasIndex("DialogueId");

                    b.ToTable("UserDialogue");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.UserProduct", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("UserProduct");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.Message", b =>
                {
                    b.HasOne("MedalsWebSystem.Models.Dialogue", "Dialogue")
                        .WithMany("Messages")
                        .HasForeignKey("DialogueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedalsWebSystem.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderUserId");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.Product", b =>
                {
                    b.HasOne("MedalsWebSystem.Models.User", "UserAdder")
                        .WithMany()
                        .HasForeignKey("UserAdderUserId");
                });

            modelBuilder.Entity("MedalsWebSystem.Models.UserDialogue", b =>
                {
                    b.HasOne("MedalsWebSystem.Models.Dialogue", "Dialogue")
                        .WithMany("UserDialogues")
                        .HasForeignKey("DialogueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedalsWebSystem.Models.User", "User")
                        .WithMany("UserDialogues")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedalsWebSystem.Models.UserProduct", b =>
                {
                    b.HasOne("MedalsWebSystem.Models.Product", "Product")
                        .WithMany("UserProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedalsWebSystem.Models.User", "User")
                        .WithMany("UserProducts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
