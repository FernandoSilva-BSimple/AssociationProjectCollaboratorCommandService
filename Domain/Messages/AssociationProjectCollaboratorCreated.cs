using Domain.Models;

namespace Domain.Messages
{
    public record AssociationProjectCollaboratorCreated(Guid id, Guid projectId, Guid collaboratorId, PeriodDate periodDate);
}

