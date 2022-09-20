using Microsoft.EntityFrameworkCore;
using System;

namespace prid_tuto.Models
{
    public class MsnContext : DbContext
    {
        public MsnContext(DbContextOptions<MsnContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>().HasIndex(m => m.FullName).IsUnique(); 

            modelBuilder.Entity<Member>().HasData(
                new Member { Pseudo = "ben", Password = "ben", FullName = "Benoît Penelle" },
                new Member { Pseudo = "bruno", Password = "bruno", FullName = "Bruno Lacroix" },
                new Member { Pseudo = "alain", Password = "alain", FullName = "Alain Silovy" },
                new Member { Pseudo = "xavier", Password = "xavier", FullName = "Xavier Pigeolet" },
                new Member { Pseudo = "boris", Password = "boris", FullName = "Boris Verhaegen" },
                new Member { Pseudo = "marc", Password = "marc", FullName = "Marc Michel" }
            );

            modelBuilder.Entity<User>().HasIndex(u => u.Pseudo).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>().HasKey(u => u.UserId).HasName("PrimaryKey"); ;
            //modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedOnAdd();


            modelBuilder.Entity<User>().HasData(
                new User { UserId = 0 ,Pseudo = "User1", Password = "user", Email = "uu.be" }/*,
                new User {Pseudo = "user2", Password = "user", Email = "u2@u.be" },
                new User { Pseudo = "User3", Password = "user", Email = "uu@yy.be" },
                new User { Pseudo = "user4", Password = "user", Email = "u2@uu.be" }*/
            ); ;
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<User> Users { get; set; }
   }
}