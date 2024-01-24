using Core.Entities.Abstract;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Security.Claims;

namespace DataAccess.DataContext.EntityFramework.Context
{
    public class AppDbContext : DbContext
    {
        private readonly Guid _authenticatedUserId;
        private readonly string _connectionStrings;

        public AppDbContext()
        {
            _connectionStrings = ServiceTool.ServiceProvider.GetService<IConfiguration>().GetConnectionString("PostgreSql");
            _authenticatedUserId = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext?.User?.Claims?.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value != null ? Guid.Parse(ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext?.User?.Claims?.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value) : Guid.Empty;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionStrings);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                #region Foreign Key Delete Behavior Configuration

                entityType.GetForeignKeys()
                          .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                          .ToList()
                          .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);

                #endregion

                #region Set HasQueryFilter For Deleted Entities

                if (entityType.ClrType.GetInterface(nameof(IDeletable)) != null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var body = Expression.Equal(
                        Expression.Property(parameter, "IsDeleted"),
                        Expression.Constant(false, typeof(bool))
                    );
                    var lambda = Expression.Lambda(body, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }

                #endregion
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entityEntry in ChangeTracker.Entries())
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        var createdOnProperty = entityEntry.Entity.GetType().GetProperty("CreatedOn");
                        if (createdOnProperty != null && createdOnProperty.PropertyType == typeof(DateTime))
                            createdOnProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);

                        var createdByProperty = entityEntry.Entity.GetType().GetProperty("CreatedBy");
                        if (createdByProperty != null && createdByProperty.PropertyType == typeof(Guid) && _authenticatedUserId != Guid.Empty)
                            createdByProperty.SetValue(entityEntry.Entity, _authenticatedUserId);

                        break;

                    case EntityState.Modified:
                        var modifiedOnProperty = entityEntry.Entity.GetType().GetProperty("ModifiedOn");
                        if (modifiedOnProperty != null && modifiedOnProperty.PropertyType == typeof(DateTime?))
                            modifiedOnProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);

                        var modifiedByProperty = entityEntry.Entity.GetType().GetProperty("ModifiedBy");
                        if (modifiedByProperty != null && modifiedByProperty.PropertyType == typeof(Guid?) && _authenticatedUserId != Guid.Empty)
                            modifiedByProperty.SetValue(entityEntry.Entity, _authenticatedUserId);

                        break;

                    case EntityState.Deleted:
                        entityEntry.State = EntityState.Modified;

                        var isDeletedProperty = entityEntry.Entity.GetType().GetProperty("IsDeleted");
                        if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
                            isDeletedProperty.SetValue(entityEntry.Entity, true);

                        var deletedOnProperty = entityEntry.Entity.GetType().GetProperty("DeletedOn");
                        if (deletedOnProperty != null && deletedOnProperty.PropertyType == typeof(DateTime?))
                            deletedOnProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);

                        var deletedByProperty = entityEntry.Entity.GetType().GetProperty("DeletedBy");
                        if (deletedByProperty != null && deletedByProperty.PropertyType == typeof(Guid?) && _authenticatedUserId != Guid.Empty)
                            deletedByProperty.SetValue(entityEntry.Entity, _authenticatedUserId);

                        break;

                    default:
                        break;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entityEntry in ChangeTracker.Entries())
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        var createdOnProperty = entityEntry.Entity.GetType().GetProperty("CreatedOn");
                        if (createdOnProperty != null && createdOnProperty.PropertyType == typeof(DateTime))
                            createdOnProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);

                        var createdByProperty = entityEntry.Entity.GetType().GetProperty("CreatedBy");
                        if (createdByProperty != null && createdByProperty.PropertyType == typeof(Guid) && _authenticatedUserId != Guid.Empty)
                            createdByProperty.SetValue(entityEntry.Entity, _authenticatedUserId);

                        break;

                    case EntityState.Modified:
                        var modifiedOnProperty = entityEntry.Entity.GetType().GetProperty("ModifiedOn");
                        if (modifiedOnProperty != null && modifiedOnProperty.PropertyType == typeof(DateTime?))
                            modifiedOnProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);

                        var modifiedByProperty = entityEntry.Entity.GetType().GetProperty("ModifiedBy");
                        if (modifiedByProperty != null && modifiedByProperty.PropertyType == typeof(Guid?) && _authenticatedUserId != Guid.Empty)
                            modifiedByProperty.SetValue(entityEntry.Entity, _authenticatedUserId);

                        break;

                    case EntityState.Deleted:
                        entityEntry.State = EntityState.Modified;

                        var isDeletedProperty = entityEntry.Entity.GetType().GetProperty("IsDeleted");
                        if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
                            isDeletedProperty.SetValue(entityEntry.Entity, true);

                        var deletedOnProperty = entityEntry.Entity.GetType().GetProperty("DeletedOn");
                        if (deletedOnProperty != null && deletedOnProperty.PropertyType == typeof(DateTime?))
                            deletedOnProperty.SetValue(entityEntry.Entity, DateTime.UtcNow);

                        var deletedByProperty = entityEntry.Entity.GetType().GetProperty("DeletedBy");
                        if (deletedByProperty != null && deletedByProperty.PropertyType == typeof(Guid?) && _authenticatedUserId != Guid.Empty)
                            deletedByProperty.SetValue(entityEntry.Entity, _authenticatedUserId);

                        break;

                    default:
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserPasswordReset> UserPasswordResets { get; set; }
    }
}