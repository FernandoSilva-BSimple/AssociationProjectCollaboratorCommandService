using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;

public interface IAssociationProjectCollaboratorRepository : IGenericRepositoryEF<IAssociationProjectCollaborator, AssociationProjectCollaborator, IAssociationProjectCollaboratorVisitor>
{
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync(Guid collaboratorId, Guid projectId, PeriodDate periodDate);
}
