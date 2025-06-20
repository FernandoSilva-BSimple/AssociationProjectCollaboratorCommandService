using Domain.Models;
using Domain.Visitor;

public class ProjectFactory : IProjectFactory
{
    public ProjectFactory() { }

    public IProject Create(Guid id)
    {
        return new Project(id);
    }

    public Project Create(IPRojectVisitor visitor)
    {
        return new Project(visitor.Id);
    }
}