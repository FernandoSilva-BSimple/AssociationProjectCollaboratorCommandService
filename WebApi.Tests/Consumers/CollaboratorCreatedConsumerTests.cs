using Application.Interfaces;
using MassTransit;
using Moq;
public class CollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Should_handle_collaborator_created_message()
    {
        var service = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorCreatedConsumer(service.Object);
        var message = new Domain.Messaging.CollaboratorCreated(Guid.NewGuid());

        var context = Mock.Of<ConsumeContext<Domain.Messaging.CollaboratorCreated>>(c => c.Message == message);

        await consumer.Consume(context);

        service.Verify(s => s.SubmitAsync(message.id), Times.Once);
    }
}
