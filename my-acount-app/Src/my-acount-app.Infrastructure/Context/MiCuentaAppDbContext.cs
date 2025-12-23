using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;

namespace MyAccountApp.Infrastructure.Context
{
    public class my_account_appAppDbContext : DbContext
    {
        public my_account_appAppDbContext(DbContextOptions<my_account_appAppDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<UserSecurity> UserSecurity { get; set; }
        public DbSet<UserAccessLog> UserAccessLog { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Sheet> Sheet { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<Vignette> Vignette { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity => { entity.HasKey(e => e.Id); });
            modelBuilder.Entity<UserSecurity>(entity => { entity.HasKey(e => e.Id); });
            modelBuilder.Entity<UserAccessLog>(entity => { entity.HasKey(e => e.Id); });
            modelBuilder.Entity<Account>(entity => { entity.HasKey(e => e.Id); });
            modelBuilder.Entity<Sheet>(entity => { entity.HasKey(e => e.Id); });
            modelBuilder.Entity<Card>(entity => { entity.HasKey(e => e.Id); });
            modelBuilder.Entity<Vignette>(entity => { entity.HasKey(e => e.Id); });
        }
    }
}
