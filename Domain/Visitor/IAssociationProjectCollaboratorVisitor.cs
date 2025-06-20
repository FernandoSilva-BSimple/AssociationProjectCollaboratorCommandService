using Domain.Models;

namespace Domain.Visitor
{
    public interface IAssociationProjectCollaboratorVisitor
    {
        Guid Id { get; }
        Guid CollaboratorId { get; }
        Guid ProjectId { get; }
        PeriodDate PeriodDate { get; }
    }
}
