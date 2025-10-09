using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Authentication;
using ShieldMyRide.Models;

namespace ShieldMyRide.Context
{
    public class MyDBContext : IdentityDbContext<ApplicationUser>
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OfficerAssignment> OfficerAssignments { get; set; }

        public DbSet<OfficersAssignment> Officers { get; set; }
        public DbSet<InsuranceClaim> InsuranceClaims { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyDocument> PolicyDocuments { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// IdentityUser ↔ Domain User one-to-one
            //modelBuilder.Entity<User>()
            //   .HasOne<ApplicationUser>()
            //   .WithOne()
            //   .HasForeignKey<User>(u => u.IdentityUserId)
            //   .OnDelete(DeleteBehavior.Cascade);

            // User → Proposal

            modelBuilder.Entity<User>()
                .HasMany(u => u.Proposals)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Payment
            modelBuilder.Entity<User>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Claim
            modelBuilder.Entity<User>()
                .HasMany(u => u.Claims)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → OfficerAssignment
            modelBuilder.Entity<User>()
                .HasMany(u => u.OfficerAssignments)
                .WithOne(o => o.Officer)
                .HasForeignKey(o => o.OfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Proposal → OfficerAssignments
            modelBuilder.Entity<Proposal>()
                .HasMany(p => p.OfficerAssignments)
                .WithOne(o => o.Proposal)
                .HasForeignKey(o => o.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Proposal → Quote (1-to-many)
            modelBuilder.Entity<Proposal>()
                 .HasMany(p => p.Quotes)
                 .WithOne(q => q.Proposal)
                 .HasForeignKey(q => q.ProposalId)
                 .OnDelete(DeleteBehavior.Restrict);

            // InsuranceClaim → OfficerAssignments
            modelBuilder.Entity<InsuranceClaim>()
                .HasMany(c => c.OfficerAssignments)
                .WithOne(o => o.Claim)
                .HasForeignKey(o => o.ClaimId)
                .OnDelete(DeleteBehavior.Restrict);



            //// Quote → Proposal (assuming Proposal has QuoteId)
            //modelBuilder.Entity<Quote>()
            //    .HasMany(q => q.Proposal)
            //    .WithOne(p => p.Quote)
            //    .HasForeignKey(p => p.QuoteId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Policy → PolicyDocument
            modelBuilder.Entity<Policy>()
                .HasMany(p => p.PolicyDocuments)
                .WithOne(d => d.Policy)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proposal>()
                .Property(p => p.ProposalStatus)
                .HasConversion<string>();

            modelBuilder.Entity<InsuranceClaim>()
                .Property(c => c.ClaimStatus)
                .HasConversion<string>();

            modelBuilder.Entity<OfficerAssignment>()
                        .Property(o => o.Status)
                        .HasConversion<string>();  // store enum as string
            modelBuilder.Entity<Proposal>()
              .HasMany(p => p.Quotes)
            .WithOne(q => q.Proposal)
            .HasForeignKey(q => q.ProposalId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsuranceClaim>()
                .HasOne(c => c.Proposal)
                .WithMany(p => p.Claims)
                .HasForeignKey(c => c.ProposalId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}