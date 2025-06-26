using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AssociationProjectCollaboratorRepositoryEF : GenericRepositoryEF<IAssociationProjectCollaborator, AssociationProjectCollaborator, AssociationProjectCollaboratorDataModel>, IAssociationProjectCollaboratorRepository
{
    private readonly IMapper _mapper;
    public AssociationProjectCollaboratorRepositoryEF(AssociationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public override IAssociationProjectCollaborator? GetById(Guid id)
    {
        var assProjectCollabDM = _context.Set<AssociationProjectCollaboratorDataModel>().FirstOrDefault(t => t.Id == id);

        if (assProjectCollabDM == null) return null;

        return _mapper.Map<AssociationProjectCollaboratorDataModel, AssociationProjectCollaborator>(assProjectCollabDM);
    }

    public override async Task<IAssociationProjectCollaborator?> GetByIdAsync(Guid id)
    {
        var assProjectCollabDM = await _context.Set<AssociationProjectCollaboratorDataModel>().FirstOrDefaultAsync(t => t.Id == id);

        if (assProjectCollabDM == null) return null;

        return _mapper.Map<AssociationProjectCollaboratorDataModel, AssociationProjectCollaborator>(assProjectCollabDM);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Set<AssociationProjectCollaboratorDataModel>().AnyAsync(x => x.Id == id);
    }

}
