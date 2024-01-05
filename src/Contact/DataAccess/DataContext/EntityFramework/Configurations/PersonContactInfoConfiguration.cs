using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.DataContext.EntityFramework.Configurations
{
    public class PersonContactInfoConfiguration : IEntityTypeConfiguration<PersonContactInfo>
    {
        public void Configure(EntityTypeBuilder<PersonContactInfo> builder)
        {
            builder.Property(x => x.PersonId).IsRequired();
            builder.Property(x => x.InfoType).IsRequired();
            builder.Property(x => x.InfoValue).IsRequired().HasMaxLength(250);
        }
    }
}