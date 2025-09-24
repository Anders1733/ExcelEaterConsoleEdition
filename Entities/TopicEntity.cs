using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.Entities
{
    public class TopicEntity
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CompetencyEntity> Competencies { get; set; }
    }
}
