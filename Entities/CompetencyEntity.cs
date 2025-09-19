using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.Entities
{
    public class CompetencyEntity
    {
        public int CompetencyId { get; set; }

        public int EmployeeId { get; set; }
        public virtual EmployeeEntity Employee { get; set; }
        public int DirectionId { get; set; }
        public virtual DirectionEntity Direction { get; set; }
        public int SectionId { get; set; }
        public virtual SectionEntity Section { get; set; }
        public int SubsectionId { get; set; }
        public virtual SubsectionEntity Subsection { get; set; }
        public int TopicId { get; set; }
        public virtual TopicEntity Topic { get; set; }

        public int CurrentLevel { get; set; } = 0;
        public int DesiredLevel { get; set; } = 0;
    }
}
