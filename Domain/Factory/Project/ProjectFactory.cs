using Domain.Models;
using Domain.Visitor;

public class ProjectFactory : IProjectFactory
{
    public ProjectFactory() { }

    public IProject Create(Guid id, PeriodDate periodDate)
    {
        return new Project(id, periodDate);
    }

    public Project Create(IProjectVisitor visitor)
    {
        return new Project(visitor.Id, visitor.PeriodDate);
    }
}