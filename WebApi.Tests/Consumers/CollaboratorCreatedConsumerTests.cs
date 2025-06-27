using Application.Interfaces;
using Domain.Messages;
using MassTransit;
using Moq;
public class CollaboratorCreatedConsumerTests
{/*
    [Fact]
    public async Task Should_handle_collaborator_created_message()
    {
        var service = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorCreatedConsumer(service.Object);
        var message = new CollaboratorCreated(Guid.NewGuid());

        var context = Mock.Of<ConsumeContext<CollaboratorCreated>>(c => c.Message == message);

        await consumer.Consume(context);

        service.Verify(s => s.SubmitAsync(message.id), Times.Once);
    }
    */
}
