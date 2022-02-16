using System;
using Domain.Entities.Account;
using Domain.Entities.News;
using Domain.Entities.Privilege;
using Domain.Entities.Product;
using Domain.Entities.Purchase;
using Domain.Entities.Rank;
using Domain.Entities.Server;
using Domain.Entities.Support;
using Domain.Entities.UserStat;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;

namespace Domain.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TypePrivilege> TypePrivileges { get; set; }
        public DbSet<Feature> Feature { get; set; }
        
        public DbSet<Rank> Rank { get; set; }
        
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Server> ServerInfo { get; set; }
        public DbSet<UserStat> UserStats { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Payment> Payment { get; set; }
        
        public DbSet<ChatRow> ChatRows { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Auth64Id).IsRequired();
               
            });

            modelBuilder.Entity<Role>().HasData(
                new Role[] 
                { 
                    new Role(1, "User"),
                    new Role(2, "Admin"),
                    new Role(3, "Owner"),
                    new Role(4, "Unban")
                });

            modelBuilder.Entity<TypePrivilege>().HasData(
                new TypePrivilege[]
                {
                    new TypePrivilege() { Id = 1, ImageSource = @"/images/privilege/vip.jpg", Lvl = 0, Name = "VIP", GroupName = "vip", Price = 150},
                    new TypePrivilege() { Id = 2, ImageSource = @"/images/privilege/vipplus.jpg", Lvl = 0, Name = "VIP PLUS", GroupName = "VipPlus", Price = 300},
                    new TypePrivilege() { Id = 3, ImageSource = @"/images/privilege/admin.jpg", Lvl = 1, Name = "Администратор", GroupName = "Admin", Price = 300},
                    new TypePrivilege() { Id = 4, ImageSource = @"/images/privilege/gladmin.jpg", Lvl = 2, Name = "Гл.Админ", GroupName = "GL.admin", Price = 500},
                    new TypePrivilege() { Id = 5, ImageSource = @"/images/privilege/gladminplus.jpg", Lvl = 3, Name = "Гл.Админ PLUS", GroupName = "GL.admin PLUS", Price = 700},
                });

            modelBuilder.Entity<Discount>().HasData(
                new Discount[]
                {
                    new Discount() { Id = 1, Percent =  0.5 }
                });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ip).HasColumnType("nvarchar(15)").IsRequired();
                entity.Property(e => e.Port).HasColumnType("int(5)").IsRequired();
            });


            modelBuilder.Entity<Server>().HasData(
                new Server[]
                {
                    new Server(1, "109.248.250.68", 27015),
                    new Server(2, "37.18.21.245", 27155),
                    new Server(3, "37.18.21.245", 27443),
                    new Server(4, "109.237.109.233", 27020)
                });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReportMessage).HasColumnType("nvarchar(255)").IsRequired();
                entity.Property(e => e.Answer).HasColumnType("nvarchar(255)");
                entity.Property(e => e.AccusedUserStatId).IsRequired();
                entity.Property(e => e.SenderUserStatId).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.DateCreation).IsRequired();
                entity.Property(e => e.ServerId).IsRequired();
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.ShortDescription).HasColumnType("nvarchar(255)").IsRequired();
                entity.Property(e => e.ImageSource).IsRequired();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PrivilegeId).HasMaxLength(11).IsRequired();
            });

            modelBuilder.Entity<UserStat>(entity =>
            {
                entity.Property(e => e.SteamAuth2).HasMaxLength(32).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
