using Domain.Models;
using Domain.Visitor;

public interface IProjectFactory
{
    IProject Create(Guid id);
    Project Create(IPRojectVisitor visitor);
}