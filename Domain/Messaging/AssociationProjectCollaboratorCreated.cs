using Domain.Models;

namespace MEssaging
{
    public record AssociationProjectCollaboratorCreated(Guid id, Guid projectId, Guid collaboratorId, PeriodDate periodDate);
}

