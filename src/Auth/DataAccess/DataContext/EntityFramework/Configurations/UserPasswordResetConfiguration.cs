using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.DataContext.EntityFramework.Configurations
{
    public class UserPasswordResetConfiguration : IEntityTypeConfiguration<UserPasswordReset>
    {
        public void Configure(EntityTypeBuilder<UserPasswordReset> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
