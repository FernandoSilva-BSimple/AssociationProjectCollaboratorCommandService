using Application.Interfaces;
using Domain.Messages;
using MassTransit;

namespace WebApi.Consumers;

public class ProjectCreatedConsumer : IConsumer<ProjectCreatedMessage>
{
    private readonly IProjectService _projectService;

    public ProjectCreatedConsumer(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task Consume(ConsumeContext<ProjectCreatedMessage> context)
    {
        Console.WriteLine(">>> [CONSUMER TRIGGERED]");

        try
        {
            Console.WriteLine("🔥 ProjectCreated received: " + context.Message.Id);
            var msg = context.Message;
            await _projectService.SubmitAsync(msg.Id, msg.PeriodDate);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            throw;
        }



    }
}