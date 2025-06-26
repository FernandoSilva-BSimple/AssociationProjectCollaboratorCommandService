using Application.Interfaces;
using Domain.Messaging;
using MassTransit;
using Moq;
using WebApi.Consumers;

public class ProjectCreatedConsumerTests
{
    [Fact]
    public async Task Should_handle_project_created_message()
    {
        var service = new Mock<IProjectService>();
        var consumer = new ProjectCreatedConsumer(service.Object);
        var message = new ProjectCreated(Guid.NewGuid());

        var context = Mock.Of<ConsumeContext<ProjectCreated>>(c => c.Message == message);

        await consumer.Consume(context);

        service.Verify(s => s.SubmitAsync(message.id), Times.Once);
    }
}
