using CourseProject_Models;
using CourseProject_Models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CourseProject_DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DbSet<Category>? Category { get; set; }
        public DbSet<Review>? Review { get; set; }
        public DbSet<ApplicationUser>? ApplicationUser { get; set; }
        public DbSet<Comment>? Comment { get; set; }
        public DbSet<Tag>? Tag { get; set; }
        public DbSet<Like>? Like { get; set; }
        public DbSet<Composition>? Composition { get; set; }
    }

    
}