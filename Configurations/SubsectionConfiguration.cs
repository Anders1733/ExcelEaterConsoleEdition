using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExcelEaterConsoleEdition.Configurations
{
    public class SubsectionConfiguration : IEntityTypeConfiguration<SubsectionEntity>
    {
        public void Configure(EntityTypeBuilder<SubsectionEntity> builder)

        {
            builder.ToTable("Subsections").HasKey(s => s.SubsectionId);

        }
    }
}
