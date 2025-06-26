using Application.Interfaces;
using Domain.Models;
using MassTransit;
using MEssaging;
using Moq;

public class AssociationProjectCollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Should_handle_association_created_message()
    {
        // Arrange
        var mockService = new Mock<IAssociationProjectCollaboratorService>();
        var consumer = new AssociationProjectCollaboratorCreatedConsumer(mockService.Object);

        var message = new AssociationProjectCollaboratorCreated(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1))));

        var context = Mock.Of<ConsumeContext<AssociationProjectCollaboratorCreated>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        mockService.Verify(s => s.CreateWithoutValidations(message.id, message.projectId, message.collaboratorId, message.periodDate), Times.Once);
    }
}
