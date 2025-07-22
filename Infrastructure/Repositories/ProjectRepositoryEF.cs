using AutoMapper;
using Domain.Models;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class ProjectRepositoryEF : GenericRepositoryEF<IProject, Project, ProjectDataModel>, IProjectRepository
{
    private readonly IMapper _mapper;
    public ProjectRepositoryEF(AssociationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public override IProject? GetById(Guid id)
    {
        var projectDataModel = _context.Set<ProjectDataModel>()
                            .FirstOrDefault(p => p.Id == id);

        if (projectDataModel == null) return null;

        return _mapper.Map<ProjectDataModel, Project>(projectDataModel);
    }

    public override async Task<IProject?> GetByIdAsync(Guid id)
    {
        var projectDataModel = await _context.Set<ProjectDataModel>()
                            .FirstOrDefaultAsync(p => p.Id == id);

        if (projectDataModel == null) return null;

        return _mapper.Map<ProjectDataModel, Project>(projectDataModel);
    }

    public async Task UpdateAsync(IProject project)
    {
        var existing = await _context.Set<ProjectDataModel>().FirstOrDefaultAsync(c => c.Id == project.Id);

        if (existing is null)
            return;

        existing.PeriodDate.InitDate = project.PeriodDate.InitDate;
        existing.PeriodDate.FinalDate = project.PeriodDate.FinalDate;

        _context.Update(existing);
        await _context.SaveChangesAsync();
    }
}