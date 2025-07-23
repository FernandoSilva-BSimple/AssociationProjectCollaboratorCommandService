using Application.Interfaces;
using Application.Services;
using Domain.Messages;
using MassTransit;

public class CollaboratorUpdatedConsumer : IConsumer<CollaboratorUpdatedMessage>
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorUpdatedConsumer(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    public async Task Consume(ConsumeContext<CollaboratorUpdatedMessage> context)
    {
        var msg = context.Message;
        await _collaboratorService.SubmitUpdateAsync(msg.Id, msg.PeriodDateTime);
    }
}