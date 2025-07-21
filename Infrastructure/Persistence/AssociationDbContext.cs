using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AssociationDbContext : DbContext
{
    public AssociationDbContext(DbContextOptions<AssociationDbContext> options)
        : base(options) { }

    public DbSet<AssociationProjectCollaboratorDataModel> Associations => Set<AssociationProjectCollaboratorDataModel>();
    public DbSet<CollaboratorDataModel> Collaborators => Set<CollaboratorDataModel>();
    public DbSet<ProjectDataModel> Projects => Set<ProjectDataModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssociationProjectCollaboratorDataModel>()
            .OwnsOne(a => a.PeriodDate);

        modelBuilder.Entity<CollaboratorDataModel>()
            .OwnsOne(c => c.PeriodDateTime);

        modelBuilder.Entity<ProjectDataModel>()
            .OwnsOne(p => p.PeriodDate);

        base.OnModelCreating(modelBuilder);
    }
}
