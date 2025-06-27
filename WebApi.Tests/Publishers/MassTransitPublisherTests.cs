using Domain.Models;
using MassTransit;
using Domain.Messages;
using Moq;
using WebApi.Publishers;

public class MassTransitPublisherTests
{
    [Fact]
    public async Task Should_publish_association_created_event()
    {
        // Arrange
        var mock = new Mock<IPublishEndpoint>();
        var publisher = new MassTransitPublisher(mock.Object);

        var id = Guid.NewGuid();
        var projId = Guid.NewGuid();
        var collabId = Guid.NewGuid();
        var period = new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(5)));

        // Act
        await publisher.PublishOrderSubmittedAsync(id, projId, collabId, period);

        // Assert
        mock.Verify(p => p.Publish(
            It.Is<AssociationProjectCollaboratorCreated>(msg =>
                msg.id == id &&
                msg.projectId == projId &&
                msg.collaboratorId == collabId &&
                msg.periodDate.Equals(period)
            ),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
