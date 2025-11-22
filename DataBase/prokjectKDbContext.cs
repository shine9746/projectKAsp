using Microsoft.EntityFrameworkCore;
using ProjectK.Models; // make sure this matches your namespace

namespace ProjectK.Data
{
    public class prokjectKDbContext : DbContext
    {
        public prokjectKDbContext(DbContextOptions<prokjectKDbContext> options) : base(options) { }

        // Define tables as DbSets:
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserPosts> Posts { get; set; }

        public DbSet<UserPostInteractionModel> UserpostInteraction { get; set; }
        public DbSet<GetAllUsersPostsModel> GetUserPosts { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GetAllUsersPostsModel>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
        }
    }
}