using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
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
                        if (createdByProperty != null && createdByProperty.PropertyType == typeof(Guid) && (Guid)createdByProperty.GetValue(entityEntry.Entity) == Guid.Empty)
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
                        break;

                    default:
                        break;
                }
            }

            return base.SaveChanges();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserPasswordReset> UserPasswordResets { get; set; }
    }
}