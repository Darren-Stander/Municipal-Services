using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Tables for the Database
        public DbSet<LocalEvent> Events { get; set; }
        public DbSet<ReportIssue> ReportIssues { get; set; }
        public DbSet<EventRsvp> EventRsvps { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Creates LocalEvent table
            modelBuilder.Entity<LocalEvent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Category).IsRequired();
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.ImageUrl).IsRequired(false); 
            });

            // This configure the ReportIssue table
            modelBuilder.Entity<ReportIssue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).IsRequired();
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            });

            // Used for EventRsvp table
            modelBuilder.Entity<EventRsvp>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // fields that user needs to fill out for rvsp for an event
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CellPhoneNumber).IsRequired().HasMaxLength(20);
                //Only optionnal feild the users needs to fill out
                entity.Property(e => e.Email).HasMaxLength(200);

                
                entity.HasOne(e => e.Event)
                    .WithMany()
                    .HasForeignKey(e => e.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Creates  index for faster searches for events and for user cell phone numbers
                entity.HasIndex(e => new { e.EventId, e.CellPhoneNumber });
            });

            // Configure ServiceRequest table
            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RequestNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Category).IsRequired();
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
                
                // Create index for faster searches by request number
                entity.HasIndex(e => e.RequestNumber).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Priority);
            });
        }
    }
}
