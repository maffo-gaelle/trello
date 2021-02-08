﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using prid_2021_002.Models;

namespace prid_2021_002.Migrations
{
    [DbContext(typeof(PridContext))]
    [Migration("20201228163012_data")]
    partial class data
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("prid_2021_002.Models.Board", b =>
                {
                    b.Property<int>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("PicturePath")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("BoardId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Boards");

                    b.HasData(
                        new
                        {
                            BoardId = 1,
                            AuthorId = 4,
                            Timestamp = new DateTime(2020, 12, 28, 17, 30, 10, 810, DateTimeKind.Local).AddTicks(6222),
                            Title = "TGPR_Grpe2"
                        },
                        new
                        {
                            BoardId = 2,
                            AuthorId = 2,
                            Timestamp = new DateTime(2020, 12, 28, 17, 30, 10, 810, DateTimeKind.Local).AddTicks(8882),
                            Title = "prwb"
                        },
                        new
                        {
                            BoardId = 3,
                            AuthorId = 2,
                            Timestamp = new DateTime(2020, 12, 28, 17, 30, 10, 810, DateTimeKind.Local).AddTicks(8976),
                            Title = "prbd"
                        });
                });

            modelBuilder.Entity("prid_2021_002.Models.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ListId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("CardId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ListId");

                    b.ToTable("Cards");

                    b.HasData(
                        new
                        {
                            CardId = 1,
                            AuthorId = 1,
                            ListId = 1,
                            Position = 0,
                            Timestamp = new DateTime(2020, 12, 28, 17, 30, 10, 815, DateTimeKind.Local).AddTicks(403),
                            Title = "Security"
                        },
                        new
                        {
                            CardId = 2,
                            AuthorId = 1,
                            ListId = 1,
                            Position = 1,
                            Timestamp = new DateTime(2020, 12, 28, 17, 30, 10, 815, DateTimeKind.Local).AddTicks(6484),
                            Title = "Signup"
                        },
                        new
                        {
                            CardId = 3,
                            AuthorId = 1,
                            ListId = 1,
                            Position = 2,
                            Timestamp = new DateTime(2020, 12, 28, 17, 30, 10, 815, DateTimeKind.Local).AddTicks(6563),
                            Title = "Login"
                        },
                        new
                        {
                            CardId = 4,
                            AuthorId = 1,
                            ListId = 1,
                            Position = 3,
                            Timestamp = new DateTime(2020, 12, 28, 17, 30, 10, 815, DateTimeKind.Local).AddTicks(6578),
                            Title = "Logout"
                        });
                });

            modelBuilder.Entity("prid_2021_002.Models.List", b =>
                {
                    b.Property<int>("ListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ListId");

                    b.HasIndex("BoardId");

                    b.ToTable("Lists");

                    b.HasData(
                        new
                        {
                            ListId = 1,
                            BoardId = 1,
                            Position = 0,
                            Title = "Baglog"
                        },
                        new
                        {
                            ListId = 2,
                            BoardId = 1,
                            Position = 1,
                            Title = "En developpement"
                        },
                        new
                        {
                            ListId = 3,
                            BoardId = 1,
                            Position = 2,
                            Title = "Test"
                        },
                        new
                        {
                            ListId = 4,
                            BoardId = 1,
                            Position = 3,
                            Title = "En production"
                        },
                        new
                        {
                            ListId = 5,
                            BoardId = 1,
                            Position = 4,
                            Title = "Deploiement"
                        });
                });

            modelBuilder.Entity("prid_2021_002.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Teamname")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            TeamId = 1,
                            Teamname = "profs"
                        });
                });

            modelBuilder.Entity("prid_2021_002.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<string>("PicturePath")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Pseudo")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            BirthDate = new DateTime(1987, 2, 6, 5, 32, 5, 0, DateTimeKind.Unspecified),
                            Email = "admin@test.com",
                            FirstName = "admin",
                            LastName = "admin",
                            Password = "admin",
                            PicturePath = "uploads/admin-user.jpg",
                            Pseudo = "admin",
                            Role = 2
                        },
                        new
                        {
                            UserId = 2,
                            Email = "ben@test.com",
                            FirstName = "Benoît",
                            LastName = "Penelle",
                            Password = "ben",
                            Pseudo = "ben",
                            Role = 0
                        },
                        new
                        {
                            UserId = 3,
                            Email = "Bruno@test.com",
                            FirstName = "Bruno",
                            LastName = "Lacroix",
                            Password = "bruno",
                            Pseudo = "bruno",
                            Role = 0
                        },
                        new
                        {
                            UserId = 4,
                            Email = "yocuba@test.com",
                            FirstName = "admin",
                            LastName = "admin",
                            Password = "yocuba",
                            Pseudo = "yocuba",
                            Role = 2
                        },
                        new
                        {
                            UserId = 5,
                            Email = "inuga@mail.com",
                            FirstName = "m",
                            LastName = "K",
                            Password = "admin",
                            Pseudo = "inuga",
                            Role = 2
                        });
                });

            modelBuilder.Entity("prid_2021_002.Models.UserBoard", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "BoardId");

                    b.HasIndex("BoardId");

                    b.ToTable("UserBoards");

                    b.HasData(
                        new
                        {
                            UserId = 3,
                            BoardId = 1
                        },
                        new
                        {
                            UserId = 2,
                            BoardId = 1
                        },
                        new
                        {
                            UserId = 1,
                            BoardId = 1
                        },
                        new
                        {
                            UserId = 1,
                            BoardId = 3
                        });
                });

            modelBuilder.Entity("prid_2021_002.UserCard", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CardId");

                    b.HasIndex("CardId");

                    b.ToTable("UserCards");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            CardId = 1
                        },
                        new
                        {
                            UserId = 1,
                            CardId = 2
                        },
                        new
                        {
                            UserId = 1,
                            CardId = 3
                        },
                        new
                        {
                            UserId = 1,
                            CardId = 4
                        });
                });

            modelBuilder.Entity("prid_2021_002.UserList", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ListId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ListId");

                    b.HasIndex("ListId");

                    b.ToTable("UserLists");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            ListId = 1
                        },
                        new
                        {
                            UserId = 1,
                            ListId = 2
                        },
                        new
                        {
                            UserId = 1,
                            ListId = 3
                        },
                        new
                        {
                            UserId = 1,
                            ListId = 4
                        });
                });

            modelBuilder.Entity("prid_2021_002.UserTeam", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("UserTeams");

                    b.HasData(
                        new
                        {
                            UserId = 3,
                            TeamId = 1
                        },
                        new
                        {
                            UserId = 2,
                            TeamId = 1
                        });
                });

            modelBuilder.Entity("prid_2021_002.Models.Board", b =>
                {
                    b.HasOne("prid_2021_002.Models.User", "Author")
                        .WithMany("Boards")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_002.Models.Card", b =>
                {
                    b.HasOne("prid_2021_002.Models.User", "Author")
                        .WithMany("Cards")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("prid_2021_002.Models.List", "List")
                        .WithMany("Cards")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_002.Models.List", b =>
                {
                    b.HasOne("prid_2021_002.Models.Board", "Board")
                        .WithMany("Lists")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_002.Models.UserBoard", b =>
                {
                    b.HasOne("prid_2021_002.Models.Board", "Board")
                        .WithMany("BoardUsers")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("prid_2021_002.Models.User", "User")
                        .WithMany("UserBoards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_002.UserCard", b =>
                {
                    b.HasOne("prid_2021_002.Models.Card", "Card")
                        .WithMany("CardUsers")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("prid_2021_002.Models.User", "User")
                        .WithMany("UserCards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_002.UserList", b =>
                {
                    b.HasOne("prid_2021_002.Models.List", "List")
                        .WithMany("ListUsers")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("prid_2021_002.Models.User", "User")
                        .WithMany("UserLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_002.UserTeam", b =>
                {
                    b.HasOne("prid_2021_002.Models.Team", "Team")
                        .WithMany("TeamUsers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("prid_2021_002.Models.User", "User")
                        .WithMany("UserTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
