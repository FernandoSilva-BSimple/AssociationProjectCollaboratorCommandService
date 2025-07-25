using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Consumers;
using Xunit;

public class ProjectUpdatedConsumerTests
{
    [Fact]
    public async Task WhenUpdatingConsumedProject_ShouldCallService()
    {
        // Arrange
        var initDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var periodDate = new PeriodDate(initDate, endDate);

        var projectServiceMock = new Mock<IProjectService>();
        var consumer = new ProjectUpdatedConsumer(projectServiceMock.Object);
        var message = new ProjectUpdatedMessage(Guid.NewGuid(), "Projeto 1", "P1", periodDate);

        var context = Mock.Of<ConsumeContext<ProjectUpdatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        projectServiceMock.Verify(
            s => s.SubmitUpdateAsync(message.Id, message.PeriodDate),
            Times.Once);
    }
}
