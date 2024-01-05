using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.DataContext.EntityFramework.Configurations
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.IpAddress).HasMaxLength(250);
            builder.Property(x => x.UserAgent).HasMaxLength(250);
            builder.Property(x => x.UserAgentVersion).HasMaxLength(250);
        }
    }
}
