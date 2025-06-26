using System;
using System.Threading.Tasks;
using Application.Services;
using MassTransit;
using MEssaging;
using Moq;
using Xunit;

public class ProjectCreatedConsumerTests
{
    [Fact]
    public async Task ShouldCallService_WhenProjectCreated()
    {
        var service = new Mock<ProjectService>();
        var consumer = new WebApi.Consumers.ProjectCreatedConsumer(service.Object);
        var message = new Domain.Messaging.ProjectCreated(Guid.NewGuid());

        var context = Mock.Of<ConsumeContext<Domain.Messaging.ProjectCreated>>(c => c.Message == message);

        await consumer.Consume(context);

        service.Verify(s => s.SubmitAsync(message.id), Times.Once);
    }
}
