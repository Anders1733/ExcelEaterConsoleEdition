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
    public class CompetencyEntityConfiguration : IEntityTypeConfiguration<CompetencyEntity>
    {
        public void Configure(EntityTypeBuilder<CompetencyEntity> builder)
        {

            builder.ToTable("Competencies").HasKey(e => e.CompetencyId);

            // Связь с DirectionEntity
            builder.HasOne(c => c.Direction).WithMany(d => d.Competencies).HasForeignKey(c => c.DirectionId);

            // Связь с SectionEntity
            builder.HasOne(c => c.Section).WithMany(s => s.Competencies).HasForeignKey(c => c.SectionId);

            // Связь с SubsectionEntity
            builder.HasOne(c => c.Subsection).WithMany(ss => ss.Competencies).HasForeignKey(c => c.SubsectionId);

            // Связь с TopicEntity
            builder.HasOne(c => c.Topic).WithMany(t => t.Competencies).HasForeignKey(c => c.TopicId);

            // Связь с EmployeeEntity
            builder.HasOne(c => c.Employee).WithMany(e => e.Competencies).HasForeignKey(c => c.EmployeeId);
        }
    }
}

