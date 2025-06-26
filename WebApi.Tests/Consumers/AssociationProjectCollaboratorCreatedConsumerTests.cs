using System;
using System.Threading.Tasks;
using Application.Services;
using MassTransit;
using MEssaging;
using Moq;
using Xunit;

public class AssociationProjectCollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task ShouldCallService_WhenEventConsumed()
    {
        // Arrange
        var mockService = new Mock<AssociationProjectCollaboratorService>();
        var consumer = new AssociationProjectCollaboratorCreatedConsumer(mockService.Object);

        var message = new AssociationProjectCollaboratorCreated(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new Domain.Models.PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1))));

        var context = Mock.Of<ConsumeContext<AssociationProjectCollaboratorCreated>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        mockService.Verify(s => s.CreateWithoutValidations(message.id, message.projectId, message.collaboratorId, message.periodDate), Times.Once);
    }
}
