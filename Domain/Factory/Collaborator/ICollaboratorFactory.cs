using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public interface ICollaboratorFactory
{
    ICollaborator Create(Guid id, PeriodDateTime periodDateTime);
    Collaborator Create(ICollaboratorVisitor visitor);
}
