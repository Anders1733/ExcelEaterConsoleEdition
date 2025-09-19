using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.Entities
{
    public class EmployeeEntity
    {
        public int EmployeeId { get; set; }
        public int UnitId { get; set; }
        public string EmployeeIdInVbpm { get; set; } = "";//пока пусть это будет email
        public string Name { get; set; } = "";

        public virtual UnitEntity? Unit { get; set; }

        public virtual ICollection<CompetencyEntity>? Competencies { get; set; }
    }
}
