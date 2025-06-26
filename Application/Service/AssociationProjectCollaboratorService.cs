using Application.DTO;
using Application.Publishers;
using Contracts;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using MassTransit;

namespace Application.Services;

public class AssociationProjectCollaboratorService
{
    private readonly IAssociationProjectCollaboratorRepository _assocRepository;
    private readonly IAssociationProjectCollaboratorFactory _factory;
    private readonly IMessagePublisher _publisher;

    public AssociationProjectCollaboratorService(
     IAssociationProjectCollaboratorRepository assocRepository,
     IAssociationProjectCollaboratorFactory factory,
     IMessagePublisher publisher)
    {
        _assocRepository = assocRepository;
        _factory = factory;
        _publisher = publisher;
    }

    public async Task<Result<AssociationProjectCollaboratorDTO>> Create(CreateAssociationProjectCollaboratorDTO dto)
    {
        IAssociationProjectCollaborator assPC;

        try
        {
            assPC = await _factory.Create(dto.ProjectId, dto.CollaboratorId, dto.PeriodDate.InitDate, dto.PeriodDate.FinalDate);
            assPC = await _assocRepository.AddAsync(assPC);
        }
        catch (ArgumentException a)
        {
            return Result<AssociationProjectCollaboratorDTO>.Failure(Error.BadRequest(a.Message));
        }
        catch (Exception e)
        {
            return Result<AssociationProjectCollaboratorDTO>.Failure(Error.BadRequest(e.Message));
        }

        await _publisher.PublishOrderSubmittedAsync(assPC.Id, assPC.ProjectId, assPC.CollaboratorId, assPC.PeriodDate);

        var result = new AssociationProjectCollaboratorDTO();
        result.Id = assPC.Id;
        result.CollaboratorId = assPC.CollaboratorId;
        result.ProjectId = assPC.ProjectId;
        result.PeriodDate = assPC.PeriodDate;

        return Result<AssociationProjectCollaboratorDTO>.Success(result);
    }

    public async Task CreateWithoutValidations(Guid id, Guid projectId, Guid collaboratorId, PeriodDate periodDate)
    {
        var exists = await _assocRepository.ExistsAsync(id);
        if (exists)
            return;
        var assPC = _factory.Create(id, projectId, collaboratorId, periodDate);
        await _assocRepository.AddAsync(assPC);
    }
}
