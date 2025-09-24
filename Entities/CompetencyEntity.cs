
namespace ExcelEaterConsoleEdition.Entities
{
    public class CompetencyEntity
    {
        public Guid CompetencyId { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual EmployeeEntity Employee { get; set; }
        public Guid DirectionId { get; set; }
        public virtual DirectionEntity Direction { get; set; }
        public Guid SectionId { get; set; }
        public virtual SectionEntity Section { get; set; }
        public Guid SubsectionId { get; set; }
        public virtual SubsectionEntity Subsection { get; set; }
        public Guid TopicId { get; set; }
        public virtual TopicEntity Topic { get; set; }

        public int CurrentLevel { get; set; } = 0;
        public int DesiredLevel { get; set; } = 0;
    }
}
