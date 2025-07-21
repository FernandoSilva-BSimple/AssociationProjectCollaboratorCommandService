using Domain.Interfaces;
using Domain.Visitor;
using Domain.Models;

namespace Domain.Factory;

public class CollaboratorFactory : ICollaboratorFactory
{
    public CollaboratorFactory() { }

    public ICollaborator Create(Guid id, PeriodDateTime periodDateTime)
    {
        return new Collaborator(id, periodDateTime);
    }

    public Collaborator Create(ICollaboratorVisitor visitor)
    {
        return new Collaborator(visitor.Id, visitor.PeriodDateTime);
    }
}
