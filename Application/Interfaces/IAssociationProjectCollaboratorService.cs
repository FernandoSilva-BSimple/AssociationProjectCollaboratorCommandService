using Application.DTO;
using Domain.Models;

namespace Application.Interfaces;

public interface IAssociationProjectCollaboratorService
{
    Task<Result<AssociationProjectCollaboratorDTO>> Create(CreateAssociationProjectCollaboratorDTO dto);
    Task CreateWithoutValidations(Guid id, Guid projectId, Guid collaboratorId, PeriodDate periodDate);

}