using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using Moq;
using Xunit;

public class CollaboratorUpdatedConsumerTests
{
    [Fact]
    public async Task Should_handle_collaborator_updated_message()
    {
        // Arrange
        var initDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var periodDateTime = new PeriodDateTime(initDate, endDate);

        var collaboratorServiceMock = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorUpdatedConsumer(collaboratorServiceMock.Object);
        var message = new CollaboratorCreatedMessage(Guid.NewGuid(), Guid.NewGuid(), periodDateTime);

        var context = Mock.Of<ConsumeContext<CollaboratorCreatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        collaboratorServiceMock.Verify(
            s => s.SubmitUpdateAsync(message.Id, message.PeriodDateTime),
            Times.Once);
    }
}
