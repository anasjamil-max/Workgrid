using Microsoft.EntityFrameworkCore;
using Workgrid.Models;

namespace Workgrid.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<WorkTask> Tasks { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrganizationMember>()
            .HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId);

        modelBuilder.Entity<OrganizationMember>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<OrganizationMember>()
            .HasIndex(x => new { x.OrganizationId, x.UserId })
            .IsUnique();

        //

        modelBuilder.Entity<Invitation>()
       .HasOne<Organization>()
       .WithMany()
       .HasForeignKey(x => x.OrganizationId);

        modelBuilder.Entity<Invitation>()
              .HasOne<User>()
              .WithMany()
              .HasForeignKey(x => x.InvitedByUserId);

        modelBuilder.Entity<Team>()
              .HasOne<Organization>()
              .WithMany()
              .HasForeignKey(x => x.OrganizationId);

     modelBuilder.Entity<TeamMember>()
    .HasOne<Team>()
    .WithMany()
    .HasForeignKey(x => x.TeamId);

        modelBuilder.Entity<TeamMember>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<TeamMember>()
    .HasIndex(x => new { x.TeamId, x.UserId })
    .IsUnique();



        //

        modelBuilder.Entity<Project>()
            .HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId);

      
        modelBuilder.Entity<Project>()
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(x => x.TeamId);

        
        modelBuilder.Entity<Project>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId);


        //

        modelBuilder.Entity<WorkTask>()
            .HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId);

      
        modelBuilder.Entity<WorkTask>()
            .HasOne<Project>()
            .WithMany()
            .HasForeignKey(x => x.ProjectId);

        
     modelBuilder.Entity<WorkTask>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.AssignedToUserId);

      modelBuilder.Entity<WorkTask>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId);

 modelBuilder.Entity<AuditLog>()
            .HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId);

             modelBuilder.Entity<AuditLog>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

    }






}





