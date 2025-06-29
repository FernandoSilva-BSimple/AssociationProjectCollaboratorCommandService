using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using Moq;
public class CollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Should_handle_collaborator_created_message()
    {

        var initDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        PeriodDateTime periodDateTime = new PeriodDateTime(initDate, endDate);

        var service = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorCreatedConsumer(service.Object);
        var message = new CollaboratorCreatedMessage(Guid.NewGuid(), Guid.NewGuid(), periodDateTime);

        var context = Mock.Of<ConsumeContext<CollaboratorCreatedMessage>>(c => c.Message == message);

        await consumer.Consume(context);

        service.Verify(s => s.SubmitAsync(message.Id), Times.Once);
    }

}
