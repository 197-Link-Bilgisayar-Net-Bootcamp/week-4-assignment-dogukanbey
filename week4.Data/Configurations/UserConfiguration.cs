using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using week4.Data.Models;

namespace week4.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserApp>
    {

        public void Configure(EntityTypeBuilder<UserApp> builder)
        {
            builder.Property(x => x.City).HasMaxLength(50);

        }

    }

}