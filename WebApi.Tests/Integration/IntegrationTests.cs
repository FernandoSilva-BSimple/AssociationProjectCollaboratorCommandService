using System;
using System.Threading.Tasks;
using Application.Services;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using MEssaging;
using Moq;
using Xunit;

public class IntegrationTests
{
    [Fact]
    public async Task ShouldConsumeAndPublishMessage()
    {
        var harness = new InMemoryTestHarness();

        var mockService = new Mock<AssociationProjectCollaboratorService>();
        var consumerHarness = harness.Consumer(() =>
            new AssociationProjectCollaboratorCreatedConsumer(mockService.Object)
        );

        await harness.Start();
        try
        {
            var msg = new AssociationProjectCollaboratorCreated(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(2)))
            );

            await harness.Bus.Publish(msg);

            Assert.True(await harness.Consumed.Any<AssociationProjectCollaboratorCreated>(), "Message was not consumed by the bus");

            mockService.Verify(s => s.CreateWithoutValidations(msg.id, msg.projectId, msg.collaboratorId, msg.periodDate), Times.Once);
        }
        finally
        {
            await harness.Stop();
        }
    }

}
