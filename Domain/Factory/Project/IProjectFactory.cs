using Domain.Models;
using Domain.Visitor;

public interface IProjectFactory
{
    IProject Create(Guid id, PeriodDate periodDate);
    Project Create(IProjectVisitor visitor);
}