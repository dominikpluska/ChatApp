using AuthApi.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string defaultAdminRoleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<Role>()
                .HasMany(x => x.UserAccount)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .IsRequired(false);

            modelBuilder.Entity<Role>().HasData(new Role
            {
                RoleId = defaultAdminRoleId,
                RoleName = "Admin"
            }, 
            new Role 
            {
                RoleName = "User"
            });

            //modelBuilder.Entity<UserAccount>()
            //    .HasMany(x => x.Role)
            //    .WithOne(x => x.UserAccount)
            //    .HasForeignKey<UserAccount>(x => x.RoleId);

            modelBuilder.Entity<UserAccount>().HasData(new UserAccount
            {
                UserName = "TestAccount",
                Email = "TestAccount@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(_configuration.GetValue<string>("DefaultAdminPassword")),
                RoleId = defaultAdminRoleId,
                IsActive = true,
            }); ;

        }
    }
}
