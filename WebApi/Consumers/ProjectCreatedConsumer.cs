using Application.Interfaces;
using Application.Services;
using Domain.Messaging;
using MassTransit;

namespace WebApi.Consumers;

public class ProjectCreatedConsumer : IConsumer<ProjectCreated>
{
    private readonly IProjectService _projectService;

    public ProjectCreatedConsumer(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task Consume(ConsumeContext<ProjectCreated> context)
    {
        var msg = context.Message;
        await _projectService.SubmitAsync(msg.id);
    }
}