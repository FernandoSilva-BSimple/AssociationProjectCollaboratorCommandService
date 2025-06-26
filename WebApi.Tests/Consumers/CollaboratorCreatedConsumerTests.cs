using System;
using System.Threading.Tasks;
using Application.Services;
using MassTransit;
using MEssaging;
using Moq;
using Xunit;
public class CollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task ShouldCallService_WhenCollaboratorCreated()
    {
        var service = new Mock<CollaboratorService>();
        var consumer = new CollaboratorCreatedConsumer(service.Object);
        var message = new Domain.Messaging.CollaboratorCreated(Guid.NewGuid());

        var context = Mock.Of<ConsumeContext<Domain.Messaging.CollaboratorCreated>>(c => c.Message == message);

        await consumer.Consume(context);

        service.Verify(s => s.SubmitAsync(message.id), Times.Once);
    }
}
