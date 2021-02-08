using System;
using Microsoft.EntityFrameworkCore;

namespace prid_2021_002.Models
{
    public class PridContext : DbContext
    {
        public PridContext(DbContextOptions<PridContext> options):base(options)
        {}
 
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<UserTeam> UserTeams {get; set; }
        public DbSet<UserBoard> UserBoards {get; set; }
        public DbSet<UserCard> UserCards {get; set; }
        public DbSet<UserList> UserLists {get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            structuralConfiguaration(modelBuilder);
            addData(modelBuilder);
        }

        private void structuralConfiguaration(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<UserTeam>().HasKey( ut => new {ut.UserId, ut.TeamId});
            modelBuilder.Entity<UserBoard>().HasKey( ub => new {ub.UserId, ub.BoardId});
            modelBuilder.Entity<UserCard>().HasKey( uc => new {uc.UserId, uc.CardId});
            modelBuilder.Entity<UserList>().HasKey( ul => new {ul.UserId, ul.ListId});

            //Board.Author(1 user) <--> User.Boards(* board)
            modelBuilder.Entity<Board>()//On met la classe de plusieurs
                        .HasOne<User>(b => b.Author) // définit la propriété de navigation pour le côté (1) de la relation
                        .WithMany(u => u.Boards)      // définit la propriété de navigation pour le côté (N) de la relation
                        .HasForeignKey(b => b.AuthorId)   //// spécifie que la clé étrangère est Board.Author.UserId
                        .OnDelete(DeleteBehavior.Restrict);

            //Board.Lists(* List) <--> List.Board(1 Board)
            modelBuilder.Entity<Board>()
                        .HasMany<List>(b => b.Lists) // définit la propriété de navigation pour le côté (1) de la relation
                        .WithOne(l => l.Board)      // définit la propriété de navigation pour le côté (N) de la relation
                        .HasForeignKey(l => l.BoardId)   //// spécifie que la clé étrangère est Board.BoardId
                        .OnDelete(DeleteBehavior.Restrict);

            //user.Cards(* cards) <--> list.Author(1 user)
            modelBuilder.Entity<Card>()
                        .HasOne<User>(c => c.Author)
                        .WithMany(u => u.Cards)
                        .HasForeignKey(c => c.AuthorId)
                        .OnDelete(DeleteBehavior.Restrict);

            //list.Cards(* cards) <--> card.List(1 liste)
            modelBuilder.Entity<Card>()
                        .HasOne<List>(c => c.List)
                        .WithMany(l => l.Cards)
                        .HasForeignKey(c => c.ListId)
                        .OnDelete(DeleteBehavior.Restrict);
            
            //1e relation N-N correspond à 2 relations 1-N
            //1) User.UsersBords(* userBords) <--> UserBoad.User(1 user)
            modelBuilder.Entity<UserBoard>()
                        .HasOne<User>(ub => ub.User)
                        .WithMany(u => u.UserBoards)
                        .HasForeignKey(ub => ub.UserId)
                        .OnDelete(DeleteBehavior.Restrict);
            
            //2) Board.BoardUsers(* BoardUsers) <--> UserBoad.Board(1 board)
            modelBuilder.Entity<UserBoard>()
                        .HasOne<Board>(ub => ub.Board)
                        .WithMany(b => b.BoardUsers)
                        .HasForeignKey(ub => ub.BoardId)
                        .OnDelete(DeleteBehavior.Restrict);


            //1e relation N-N correspond à 2 relations 1-N
            //1) User.UserCards(* UserCards) <--> UserCard.User(1 user)
            modelBuilder.Entity<UserCard>()
                        .HasOne<User>(uc => uc.User)
                        .WithMany(u => u.UserCards)
                        .HasForeignKey(uc => uc.UserId)
                        .OnDelete(DeleteBehavior.Restrict);
            
            //2) Card.CardUsers(* CardUsers) <--> UserCard.Card(1 Card)
            modelBuilder.Entity<UserCard>()
                        .HasOne<Card>(uc => uc.Card)
                        .WithMany(c => c.CardUsers)
                        .HasForeignKey(uc => uc.CardId)
                        .OnDelete(DeleteBehavior.Restrict);


            //1e relation N-N correspond à 2 relations 1-N
            //1) User.UserLists(* UserLists) <--> UserList.User(1 user)
            modelBuilder.Entity<UserList>()
                        .HasOne<User>(ul => ul.User)
                        .WithMany(u => u.UserLists)
                        .HasForeignKey(ul => ul.UserId)
                        .OnDelete(DeleteBehavior.Restrict);
            
            //2) List.ListUsers(* ListUsers) <--> UserList.List(1 List)
            modelBuilder.Entity<UserList>()
                        .HasOne<List>(ul => ul.List)
                        .WithMany(l => l.ListUsers)
                        .HasForeignKey(ul => ul.ListId)
                        .OnDelete(DeleteBehavior.Restrict);


            //1e relation N-N correspond à 2 relations 1-N
            //1) User.UserTeams(* UserTeams) <--> UserTeam.User(1 user)
            modelBuilder.Entity<UserTeam>()
                        .HasOne<User>(ut => ut.User)
                        .WithMany(u => u.UserTeams)
                        .HasForeignKey(ut => ut.UserId)
                        .OnDelete(DeleteBehavior.Restrict);
            
            //2) Team.TeamUsers(* TeamUsers) <--> UserTeam.Team(1 List)
            modelBuilder.Entity<UserTeam>()
                        .HasOne<Team>(ut => ut.Team)
                        .WithMany(t => t.TeamUsers)
                        .HasForeignKey(ut => ut.TeamId)
                        .OnDelete(DeleteBehavior.Restrict);
        }

        private void addData(ModelBuilder modelBuilder) 
        {
            addUsers(modelBuilder);
            addTeams(modelBuilder);
            addBoards(modelBuilder);
            addLists(modelBuilder);
            addCards(modelBuilder);
            addUserTeams(modelBuilder);
            addUserBoads(modelBuilder);
            addUserLists(modelBuilder);
            addUserCards(modelBuilder);
        }

        public void addUsers(ModelBuilder modelBuilder) 
        {
            int id = 1;
            modelBuilder.Entity<User>().HasData(
                new User() { UserId = id++, Pseudo = "admin", Password = "admin", Email = "admin@test.com",LastName = "admin", FirstName = "admin", Role = Role.Admin, PicturePath = "uploads/admin-user.jpg", BirthDate= new DateTime(1987,2,6,5,32,5) },
                new User() { UserId = id++, Pseudo = "ben", Password = "ben", Email = "ben@test.com",LastName = "Penelle", FirstName = "Benoît" },
                new User() { UserId = id++, Pseudo = "bruno", Password = "bruno", Email = "Bruno@test.com", LastName = "Lacroix", FirstName = "Bruno" },
                new User() { UserId = id++, Pseudo = "yocuba", Password = "yocuba", Email = "yocuba@test.com",LastName = "admin", FirstName = "admin", Role = Role.Admin },
                new User() { UserId = id++, Pseudo = "inuga", Password = "admin", Email = "inuga@mail.com",LastName = "K", FirstName = "m", Role = Role.Admin }
            );
        }

        public void addTeams(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Team>().HasData(new Team(){TeamId=1,Teamname="profs"});
        }

        private void addBoards(ModelBuilder modelBuilder) 
        {
            int id = 1;
            modelBuilder.Entity<Board>().HasData(
                new Board() {BoardId = id++, Title = "TGPR_Grpe2", Timestamp = DateTime.Now, AuthorId = 4},
                new Board() {BoardId = id++, Title = "prwb", Timestamp = DateTime.Now, AuthorId = 2},
                new Board() {BoardId = id++, Title = "prbd", Timestamp = DateTime.Now, AuthorId = 2}
            );
        }

        private void addLists(ModelBuilder modelBuilder) 
        {
            int id = 1;
            modelBuilder.Entity<List>().HasData (
                new List() { ListId = id++, Position = 0, Title = "Baglog", BoardId = 1 },
                new List() { ListId = id++, Position = 1, Title = "En developpement", BoardId = 1 },
                new List() { ListId = id++, Position = 2, Title = "Test", BoardId = 1},
                new List() { ListId = id++, Position = 3, Title = "En production", BoardId = 1},
                new List() { ListId = id++, Position = 4, Title = "Deploiement", BoardId = 1}
            );
        }

        private void addCards(ModelBuilder modelBuilder) 
        {
            int id = 1;
            modelBuilder.Entity<Card>().HasData(
                new Card() { CardId = id++, Title = "Security", Position = 0, Timestamp = DateTime.Now, AuthorId = 1, ListId = 1},
                new Card() { CardId = id++, Title = "Signup" , Position = 1, Timestamp = DateTime.Now, AuthorId = 1, ListId = 1},
                new Card() { CardId = id++, Title = "Login" , Position = 2, Timestamp = DateTime.Now, AuthorId = 1, ListId = 1 },
                new Card() { CardId = id++, Title = "Logout" , Position = 3, Timestamp = DateTime.Now, AuthorId = 1, ListId = 1 }
            );
        }

        private void addUserTeams(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTeam>().HasData(
                new UserTeam() {UserId = 3, TeamId = 1},
                new UserTeam() {UserId = 2, TeamId = 1}
            );
        }

        private void addUserBoads(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBoard>().HasData(
                new UserBoard() {UserId = 3, BoardId = 1},
                new UserBoard() {UserId = 2, BoardId = 1},
                new UserBoard() {UserId = 1, BoardId = 1},
                new UserBoard() {UserId = 1, BoardId = 3}
            );
        }

        private void addUserCards(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCard>().HasData(
                new UserCard() {UserId = 1, CardId = 1},
                new UserCard() {UserId = 1, CardId = 2},
                new UserCard() {UserId = 1, CardId = 3},
                new UserCard() {UserId = 1, CardId = 4}
            );
        }

        private void addUserLists(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserList>().HasData(
                new UserList() {UserId = 1, ListId = 1},
                new UserList() {UserId = 1, ListId = 2},
                new UserList() {UserId = 1, ListId = 3},
                new UserList() {UserId = 1, ListId = 4}
            );
        }
    }
}