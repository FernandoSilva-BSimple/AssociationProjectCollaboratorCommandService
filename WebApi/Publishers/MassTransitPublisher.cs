using Application.Publishers;
using Domain.Messaging;
using Domain.Models;
using MassTransit;
using MEssaging;

namespace WebApi.Publishers;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishOrderSubmittedAsync(Guid Id, Guid projectId, Guid collaboratorId, PeriodDate periodDate)
    {
        await _publishEndpoint.Publish(new AssociationProjectCollaboratorCreated(Id, projectId, collaboratorId, periodDate));
    }
}
