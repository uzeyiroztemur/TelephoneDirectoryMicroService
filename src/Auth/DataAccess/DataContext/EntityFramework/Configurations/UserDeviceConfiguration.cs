using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.DataContext.EntityFramework.Configurations
{
    public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(EntityTypeBuilder<UserDevice> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.IpAddress).HasMaxLength(250);
            builder.Property(x => x.OS).HasMaxLength(250);
            builder.Property(x => x.OSVersion).HasMaxLength(250);
            builder.Property(x => x.Model).HasMaxLength(250);
            builder.Property(x => x.Name).HasMaxLength(250);
            builder.Property(x => x.BrowserName).HasMaxLength(250);
            builder.Property(x => x.DeviceId).HasMaxLength(250);
        }
    }
}
