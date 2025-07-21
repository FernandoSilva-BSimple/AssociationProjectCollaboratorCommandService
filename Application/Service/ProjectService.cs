using Application.Interfaces;
using Domain.IRepository;
using Domain.Models;

public class ProjectService : IProjectService
{
    public IProjectRepository _projectRepository;
    public IProjectFactory _projectFactory;

    public ProjectService(IProjectRepository projectRepository, IProjectFactory projectFactory)
    {
        _projectRepository = projectRepository;
        _projectFactory = projectFactory;
    }

    public async Task SubmitAsync(Guid id, PeriodDate periodDate)
    {
        IProject project;

        project = _projectFactory.Create(id, periodDate);
        await _projectRepository.AddAsync(project);
    }
}