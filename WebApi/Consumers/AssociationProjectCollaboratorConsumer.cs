using Application.Interfaces;
using Application.Services;
using MassTransit;
using MEssaging;

public class AssociationProjectCollaboratorCreatedConsumer : IConsumer<AssociationProjectCollaboratorCreated>
{
    private readonly IAssociationProjectCollaboratorService _assocService;

    public AssociationProjectCollaboratorCreatedConsumer(IAssociationProjectCollaboratorService assPCService)
    {
        _assocService = assPCService;
    }

    public async Task Consume(ConsumeContext<AssociationProjectCollaboratorCreated> context)
    {
        var message = context.Message;
        await _assocService.CreateWithoutValidations(message.id, message.projectId, message.collaboratorId, message.periodDate);
    }
}