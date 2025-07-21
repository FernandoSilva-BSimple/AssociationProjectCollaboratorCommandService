using Domain.Models;
using Domain.Visitor;

public class ProjectDataModel : IProjectVisitor
{
    public Guid Id { get; set; }
    public PeriodDate PeriodDate { get; set; }
    public ProjectDataModel() { }

    public ProjectDataModel(IProject project)
    {
        Id = project.Id;
        PeriodDate = project.PeriodDate;
    }
}