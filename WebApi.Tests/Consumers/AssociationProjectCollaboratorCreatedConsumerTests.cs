using Application.Interfaces;
using Domain.Models;
using MassTransit;
using Moq;
using Domain.Messages;

public class AssociationProjectCollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Should_handle_association_created_message()
    {
        // Arrange
        var mockService = new Mock<IAssociationProjectCollaboratorService>();
        var consumer = new AssociationProjectCollaboratorCreatedConsumer(mockService.Object);

        var message = new AssociationProjectCollaboratorCreatedMessage(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1))));

        var context = Mock.Of<ConsumeContext<AssociationProjectCollaboratorCreatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        mockService.Verify(s => s.CreateWithoutValidations(message.Id, message.ProjectId, message.CollaboratorId, message.PeriodDate), Times.Once);
    }
}
