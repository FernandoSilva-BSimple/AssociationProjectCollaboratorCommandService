using Application.Interfaces;
using Application.Services;
using Domain.Messages;
using MassTransit;

public class ProjectUpdatedConsumer : IConsumer<ProjectCreatedMessage>
{
    private readonly IProjectService _projectService;

    public ProjectUpdatedConsumer(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task Consume(ConsumeContext<ProjectCreatedMessage> context)
    {
        var msg = context.Message;
        await _projectService.SubmitUpdateAsync(msg.Id, msg.PeriodDate);
    }
}