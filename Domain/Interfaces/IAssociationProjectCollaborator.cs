using System;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IAssociationProjectCollaborator
    {
        Guid Id { get; }
        Guid CollaboratorId { get; }
        Guid ProjectId { get; }
        PeriodDate PeriodDate { get; }
    }
}
