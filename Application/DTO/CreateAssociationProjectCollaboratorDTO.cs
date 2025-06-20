using Domain.Models;

namespace Application.DTO;

public record CreateAssociationProjectCollaboratorDTO
{
    public Guid CollaboratorId { get; set; }
    public Guid ProjectId { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public CreateAssociationProjectCollaboratorDTO()
    {

    }
}