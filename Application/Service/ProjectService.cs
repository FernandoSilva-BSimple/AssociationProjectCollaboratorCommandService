using Application.Interfaces;
using Domain.IRepository;

public class ProjectService : IProjectService
{
    public IProjectRepository _projectRepository;
    public IProjectFactory _projectFactory;

    public ProjectService(IProjectRepository projectRepository, IProjectFactory projectFactory)
    {
        _projectRepository = projectRepository;
        _projectFactory = projectFactory;
    }

    public async Task SubmitAsync(Guid id)
    {
        IProject project;

        project = _projectFactory.Create(id);
        await _projectRepository.AddAsync(project);
    }
}