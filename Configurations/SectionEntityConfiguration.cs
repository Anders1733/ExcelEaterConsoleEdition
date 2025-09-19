using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExcelEaterConsoleEdition.Configurations
{
    public class SectionEntityConfiguration : IEntityTypeConfiguration<SectionEntity>
    {
        public void Configure(EntityTypeBuilder<SectionEntity> builder)

        {
            builder.ToTable("Sections").HasKey(s => s.SectionId); 

        }
    }
}
