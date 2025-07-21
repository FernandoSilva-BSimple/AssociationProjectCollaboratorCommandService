using Application.Interfaces;
using Application.Services;
using Domain.Messages;
using MassTransit;

public class CollaboratorCreatedConsumer : IConsumer<CollaboratorCreatedMessage>
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorCreatedConsumer(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    public async Task Consume(ConsumeContext<CollaboratorCreatedMessage> context)
    {
        var msg = context.Message;
        await _collaboratorService.SubmitAsync(msg.Id, msg.PeriodDateTime);
    }
}