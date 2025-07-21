using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Consumers;

public class ProjectCreatedConsumerTests
{
    [Fact]
    public async Task Should_handle_project_created_message()
    {

        var initDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        PeriodDate periodDate = new PeriodDate(initDate, endDate);

        var service = new Mock<IProjectService>();
        var consumer = new ProjectCreatedConsumer(service.Object);
        var message = new ProjectCreatedMessage(Guid.NewGuid(), "Projeto 1", "P1", periodDate);

        var context = Mock.Of<ConsumeContext<ProjectCreatedMessage>>(c => c.Message == message);

        await consumer.Consume(context);

        service.Verify(s => s.SubmitAsync(message.Id, message.PeriodDate), Times.Once);
    }
}
