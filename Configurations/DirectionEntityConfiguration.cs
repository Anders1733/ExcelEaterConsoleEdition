using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.Configurations
{
    public class DirectionEntityConfiguration : IEntityTypeConfiguration<DirectionEntity>
    {
        public void Configure(EntityTypeBuilder<DirectionEntity> builder)

        {
            builder.ToTable("Directions").HasKey(e => e.DirectionId);

        }
    }
}