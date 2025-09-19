using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.Configurations
{
    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeEntity> builder)

        {
            builder.ToTable("Employees").HasKey(e => e.EmployeeId);

            builder.HasOne(e => e.Unit)
                .WithMany(u => u.Employees)
                .HasForeignKey(e => e.UnitId);
        }
    }
}
