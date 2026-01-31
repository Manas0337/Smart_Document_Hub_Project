using Microsoft.EntityFrameworkCore;
using SmartDocumentHub.Models;

namespace SmartDocumentHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }
        public DbSet<InlineComment> InlineComments { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Document -> Versions (CASCADE OK)
            modelBuilder.Entity<DocumentVersion>()
                .HasOne(v => v.Document)
                .WithMany(d => d.Versions)
                .HasForeignKey(v => v.DocId)
                .OnDelete(DeleteBehavior.Cascade);

            // Version -> Comments (CASCADE OK)
            modelBuilder.Entity<InlineComment>()
                .HasOne(c => c.DocumentVersion)
                .WithMany(v => v.InlineComments)
                .HasForeignKey(c => c.VersionId)
                .OnDelete(DeleteBehavior.Cascade);

            // User relations (NO CASCADE ANYWHERE)
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Owner)
                .WithMany()
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DocumentVersion>()
                .HasOne(v => v.CreatedBy)
                .WithMany()
                .HasForeignKey(v => v.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InlineComment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
