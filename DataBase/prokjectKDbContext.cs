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
        public DbSet<UserCommentsModel> UserComments { get; set; }

        public DbSet<GetCommentsModel> GetComments { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GetAllUsersPostsModel>(entity =>
            {
                modelBuilder.Entity<GetCommentsModel>().HasNoKey();
                entity.HasNoKey();
                entity.ToView(null);
            });
        }
    }
}