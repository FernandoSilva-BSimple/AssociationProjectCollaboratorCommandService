namespace Contracts;

public record AssociationCreated
(
    Guid Id,
    Guid CollaboratorId,
    Guid ProjectId,
    DateOnly InitDate,
    DateOnly FinalDate
);
