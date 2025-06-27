using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using Moq;

public class IntegrationTests
{
    [Fact]
    public async Task ShouldConsumeAndPublishMessage()
    {
        var harness = new InMemoryTestHarness();

        var mockService = new Mock<IAssociationProjectCollaboratorService>();
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

            Assert.True(await harness.Consumed.Any<AssociationProjectCollaboratorCreated>());

            mockService.Verify(s =>
                s.CreateWithoutValidations(
                    msg.id,
                    msg.projectId,
                    msg.collaboratorId,
                    It.Is<PeriodDate>(p =>
                        p.InitDate == msg.periodDate.InitDate &&
                        p.FinalDate == msg.periodDate.FinalDate
                    )),
                Times.Once
            );
        }
        finally
        {
            await harness.Stop();
        }
    }

}
