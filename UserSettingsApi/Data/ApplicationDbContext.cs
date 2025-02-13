using Microsoft.EntityFrameworkCore;
using UserSettingsApi.Models;

namespace UserSettingsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<FriendsList> FriendsLists { get; set; }
        public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FriendsList>();
            modelBuilder.Entity<BlackList>();
            modelBuilder.Entity<Chat>();
            modelBuilder.Entity<FriendRequest>();

        }

    }
}
