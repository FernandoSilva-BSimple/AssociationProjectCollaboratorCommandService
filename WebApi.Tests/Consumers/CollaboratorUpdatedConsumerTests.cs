using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using Moq;
using Xunit;

public class CollaboratorUpdatedConsumerTests
{
    [Fact]
    public async Task WhenUpdatingConsumedCollaborator_ShouldCallService()
    {
        // Arrange
        var initDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var periodDateTime = new PeriodDateTime(initDate, endDate);

        var collaboratorServiceMock = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorUpdatedConsumer(collaboratorServiceMock.Object);
        var message = new CollaboratorUpdatedMessage(Guid.NewGuid(), Guid.NewGuid(), periodDateTime);

        var context = Mock.Of<ConsumeContext<CollaboratorUpdatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        collaboratorServiceMock.Verify(
            s => s.SubmitUpdateAsync(message.Id, message.PeriodDateTime),
            Times.Once);
    }
}
