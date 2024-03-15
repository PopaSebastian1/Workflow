using Licenta.Server.DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.DataLayer.Utils
{
    public class AppDbContext: IdentityDbContext<User,IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public override DbSet<User> Users { get; set; }= default!;
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueStatus> IssuesStatus { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Skill>()
                .Property(s => s.Level)
                .HasConversion<string>(); // Converteste enum-ul la string pentru stocare in baza de date
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Members)
                .WithMany(u => u.Projects)
                .UsingEntity(j => j.ToTable("UserProjects"));
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId);
            modelBuilder.Entity<Issue>()
                 .HasOne(i => i.Assignee)
                 .WithMany(u => u.Issues)
                 .HasForeignKey(i => i.AssigneeId)
                 .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
    