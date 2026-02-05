using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets — EXACTLY match DB tables
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }
        public DbSet<InlineComment> InlineComments { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    }
}
